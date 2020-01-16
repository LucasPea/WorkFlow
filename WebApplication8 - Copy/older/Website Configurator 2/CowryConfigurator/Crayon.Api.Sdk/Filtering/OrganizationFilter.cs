﻿namespace Crayon.Api.Sdk.Filtering
{
    public class OrganizationFilter : IHttpFilter
    {
        public OrganizationFilter()
        {
            Page = 1;
            PageSize = 50;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }

        public string ToQueryString()
        {
            return this.ToQuery();
        }
    }
}