using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            logger.LogError("error logged");
            logger.LogWarning("warning logged");
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
