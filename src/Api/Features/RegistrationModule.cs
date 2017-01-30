﻿using Nancy;

namespace SiteWatchman.Api.Features
{
    public class RegistrationModule : NancyModule
    {
        public RegistrationModule() : base("/registrations")
        {
            Get["/"] = parameters => Gets(parameters);
            Get["/{id}"] = parameters => "Details of a single registration";
            Post["/"] = parameters => "Create a new registration";
            Patch["/{id}"] = parameters => "Patch a single registration";
            Delete["/{id}"] = parameters => "Delete a single registration";
        }

        private object Gets(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}