using Akka.Actor;
using AkkaPakka.Messages;
using System;
using System.Collections.Generic;
using Akka.DI.Core;

namespace AkkaPakka.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private ILogger _logger;
        private readonly Dictionary<int, IActorRef> _users;

        public UserCoordinatorActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteVerbose("Creating a UserCoordinatorActor");

            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateChildUserIfNotExists(message.UserId);

                var childActorRef = _users[message.UserId];

                childActorRef.Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                CreateChildUserIfNotExists(message.UserId);

                var childActorRef = _users[message.UserId];

                childActorRef.Tell(message);
            });
        }

        private void CreateChildUserIfNotExists(int userId)
        {
            if (_users.ContainsKey(userId))
                return;

            var userActorProps = Context.DI().Props<UserActor>();

            var newChildActorRef = Context.ActorOf(userActorProps, $"User{userId}");

            _users.Add(userId, newChildActorRef);

            _logger.WriteDebug($"{Self.Path.Name} created new child UserActor for id {userId} (total count: {_users.Count})");
        }

        #region lifecycle hooks

        protected override void PreStart()
        {
            _logger.WriteDebug($"{Self.Path.Name} PreStart");

            base.PreStart();
        }

        protected override void PostStop()
        {
            _logger.WriteDebug($"{Self.Path.Name} PostStop");

            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.WriteError($"{Self.Path.Name} PreRestart: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.WriteError($"{Self.Path.Name} PostRestart: {reason}");

            base.PostRestart(reason);
        }

        #endregion
    }
}
