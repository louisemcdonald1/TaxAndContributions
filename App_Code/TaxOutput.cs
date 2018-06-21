using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TaxOutput
/// </summary>
public class TaxOutput
{
    public double PersonalAllowance { get; set; }
    public double TaxBasicRate { get; set; }
    public double TaxHigherRate { get; set; }
    public double TaxAdditionalRate { get; set; }
    public double NationalInsurance { get; set; }
    public double TotalDeductions { get; set; }
    public double NetSalary { get; set; }
    public double ChildcareVoucherAmount { get; set; }
    public double PensionAnnualAmount { get; set; }
    public double GrossSalaryInput { get; set; }
    public double StudentLoanAnnualDeduction { get; set; }
    public double PensionsContribution { get; set; }
    public double HealthContribution { get; set; }
    public double EducationContribution { get; set; }
    public double DefenceContribution { get; set; }
    public double WelfareContribution { get; set; }
    public double ProtectionContribution { get; set; }
    public double TransportContribution { get; set; }
    public double GeneralContribution { get; set; }
    public double OtherContribution { get; set; }
    public double InterestContribution { get; set; }

    public TaxOutput()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}