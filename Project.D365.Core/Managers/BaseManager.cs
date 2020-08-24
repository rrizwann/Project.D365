using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Project.D365.Core.Managers
{
    public class BaseManager
    {
        protected ITracingService TracingService { get; private set; }
        protected IOrganizationService OrganizationService { get; private set; }

        public BaseManager()
        {

        }

        public BaseManager(IOrganizationService service)
        {
            OrganizationService = service;
        }

        public BaseManager(IOrganizationService service, ITracingService tracingService)
        {
            TracingService = tracingService;
            OrganizationService = service;
        }

        protected OrganizationRequest GetOrganizationRequest(Entity oEntity)
        {
            if (oEntity.Id == Guid.Empty)
            {
                return new CreateRequest()
                {
                    Target = oEntity
                };
            }
            else
            {
                return new UpdateRequest()
                {
                    Target = oEntity
                };
            }
        }

        protected ExecuteMultipleResponse ExecuteMultiple(OrganizationRequestCollection colRequest)
        {
            ExecuteMultipleRequest request = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = colRequest
            };

            ExecuteMultipleResponse response = OrganizationService.Execute(request) as ExecuteMultipleResponse;
            return response;
        }

        protected string GetExecuteMultipleFaultLog(ExecuteMultipleResponse response)
        {
            StringBuilder sbLogFileData = new StringBuilder();

            IList<OrganizationServiceFault> faults = response.Responses
                .Where(item => item.Fault != null)
                .Select(item => item.Fault)
                .ToList();

            foreach (OrganizationServiceFault fault in faults)
            {
                sbLogFileData.Append(fault.InnerFault == null ? fault.Message : fault.InnerFault.Message);
            }

            return sbLogFileData.ToString();
        }
    }
}
