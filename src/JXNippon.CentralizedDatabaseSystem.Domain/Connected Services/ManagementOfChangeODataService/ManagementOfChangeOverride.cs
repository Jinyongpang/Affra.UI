using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using Microsoft.OData.Client;

namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges
{
    public partial class ManagementOfChangeRecord
    {
        [IgnoreClientProperty]
        public DateTime CreateDateTimeUI
        {
            get { return this.CreatedDateTime.ToLocalDateTime(); }
            set { this.CreatedDateTime = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public string ExtensionsString 
        { 
            get { return string.Join(',', this.Extensions?.Select(x => x.ReviewDateUI.ToLocalTime().ToString("d"))); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Identifications
{
    public partial class Identification
    {
        [IgnoreClientProperty]
        public DateTime ExpiryDateUI
        {
            get { return this.ExpiryDate.ToLocalDateTime(); }
            set { this.ExpiryDate = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.AuthorisationAndApprovals
{
    public partial class AuthorisationAndApproval
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CloseOuts
{
    public partial class CloseOut
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CommunicationAndImplementations
{
    public partial class CommunicationAndImplementation
    {
        [IgnoreClientProperty]
        public DateTime AgreedByDateUI
        {
            get { return this.AgreedByDate.ToLocalDateTime(); }
            set { this.AgreedByDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime PreparedByDateUI
        {
            get { return this.PreparedByDate.ToLocalDateTime(); }
            set { this.PreparedByDate = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Endorsements
{
    public partial class Endorsement
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Extensions
{
    public partial class Extension
    {
        [IgnoreClientProperty]
        public DateTime ReviewDateUI
        {
            get { return this.ReviewDate.ToLocalDateTime(); }
            set { this.ReviewDate = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.OperationInstructions
{
    public partial class OperationInstructionRecord
    {
        [IgnoreClientProperty]
        public DateTime CreateDateTimeUI
        {
            get { return this.CreatedDateTime.ToLocalDateTime(); }
            set { this.CreatedDateTime = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime EstimatedDurationDateTimeUI
        {
            get { return this.EstimatedDurationDateTime.ToLocalDateTime(); }
            set { this.EstimatedDurationDateTime = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime PreparedByDateTimeUI
        {
            get { return this.PreparedByDateTime.ToLocalDateTime(); }
            set { this.PreparedByDateTime = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime EndorsedByDateTimeUI
        {
            get { return this.EndorsedByDateTime.ToLocalDateTime(); }
            set { this.EndorsedByDateTime = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime ApprovedByDateTimeUI
        {
            get { return this.ApprovedByDateTime.ToLocalDateTime(); }
            set { this.ApprovedByDateTime = value.ToUniversalTime(); }
        }
    }
}
