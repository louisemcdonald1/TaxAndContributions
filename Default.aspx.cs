using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Drawing;

public partial class _Default : Page
{
    //2017-2018 tax thresholds and rates - all constants as they must not 
    //be changed in the code
    const double personalAllowanceLimit = 11859;                //updated for 2018-19
    const double basicRate = 0.2;
    const double higherRateThreshold = 46359;                   //updated for 2018-19
    const double higherRate = 0.4;
    const double additionalRateThreshold = 150000;
    const double additionalRate = 0.45;
    const double personalAllowanceAdjustmentThreshold = 100000;
    const double nationalInsuranceThreshold = 8424;             //updated for 2018-19
    const double nationalInsuranceRate = 0.12;
    const double nationalInsuranceUpperEarningsLimit = 46350;   //updated for 2018-19
    const double nationalInsuranceUpperEarningsRate = 0.02;
    const double basicRateMaximumChildcareVouchers = 243;
    const double higherRateMaximumChildcareVouchers = 127;
    const double additionalRateMaximumChildcareVouchers = 110;
    const double studentLoanPlan1Threshold = 18330;             //updated for 2018-19
    const double studentLoanPlan2Threshold = 25000;             //updated for 2018-19
    const double studentLoanRepaymentRate = 0.09;

    //government spending data - also constants
    const double pensionsSpending = 0.26;
    const double healthSpending = 0.23;
    const double educationSpending = 0.07;
    const double defenceSpending = 0.07;
    const double welfareSpending = 0.09;
    const double protectionSpending = 0.03;
    const double transportSpending = 0.03;
    const double generalSpending = 0.02;
    const double otherSpending = 0.12;
    const double interestSpending = 0.08;

    //variables that are needed by more than one event handler
    //static double childcareVoucherAmount = 0;
    //static double pensionAnnualAmount = 0;
    //static double grossSalaryInput = 0;
    //static double pensionPercentageAmount = 0;
    //static double pensionMonthlyAmount = 0;
    //static double studentLoanAnnualDeduction = 0;
    static bool validInput = true;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCalculateTax_Click(object sender, EventArgs e)
    {
        double grossSalaryInput = 0;
        double childcareVoucherMonthlyAmount = 0;
        double pensionAnnualAmount = 0;
        double studentLoanAnnualPayment = 0;

        //get input from server controls
        bool validSalaryInput = Double.TryParse(tbGrossSalary.Text, out grossSalaryInput);
        bool validCcvInput = Double.TryParse(tbChildcareVouchers.Text, out childcareVoucherMonthlyAmount);
        
        //check that the childcare voucher amount is within the rules ------------------------
        MethodReturn methodReturn = new MethodReturn();

        if (validCcvInput)
        {
            methodReturn = CheckChildCareVoucherAmount(grossSalaryInput, childcareVoucherMonthlyAmount);
        }

        if (!String.IsNullOrEmpty(methodReturn.errorMessage))
        {
            //correct childcare voucher amount for calculations
            childcareVoucherMonthlyAmount = methodReturn.returnValue;
            //display an error message
            lblCcvError.Text = methodReturn.errorMessage;
            //correct childcare voucher amount in textbox
            tbChildcareVouchers.Text = methodReturn.returnValue.ToString();
            tbChildcareVouchers.ForeColor = Color.Red;
        }
        else
        {
            //maybe these bits do belong in a _textChanged event handler
            tbChildcareVouchers.ForeColor = Color.Black;
            lblCcvError.Text = "";
        }
        //-------------------------------------------------------------------------------------------
        //check that pension input is correct and get the annual contribution

        methodReturn = CheckPensionInputAndCalculate(tbPension.Text, grossSalaryInput);
        pensionAnnualAmount = methodReturn.returnValue;
        lblPensionError.Text = methodReturn.errorMessage;

        //calculate student loan payment
        methodReturn = CalculateStudentLoanPayment(grossSalaryInput, ddlStudentLoan.SelectedValue);
        studentLoanAnnualPayment = methodReturn.returnValue;
        //-------------------------------------------------------------------
        if (validInput)
        {
            TaxOutput taxOutput = CalculateTax(grossSalaryInput, childcareVoucherMonthlyAmount, pensionAnnualAmount, studentLoanAnnualPayment);
            //output displayed with currency symbols and thousands separator
            lblChildcareVoucherAmount.Text = taxOutput.ChildcareVoucherAmount.ToString("C");
            lblStudentLoanAmount.Text = taxOutput.StudentLoanAnnualDeduction.ToString("C");
            lblpensionAnnualAmount.Text = taxOutput.PensionAnnualAmount.ToString("C");
            lblPersonalAllowance.Text = taxOutput.PersonalAllowance.ToString("C");
            lblBasicRateTax.Text = taxOutput.TaxBasicRate.ToString("C");
            lblHigherRateTax.Text = taxOutput.TaxHigherRate.ToString("C");
            lblAdditionalRateTax.Text = taxOutput.TaxAdditionalRate.ToString("C");
            lblNationalInsurancePaid.Text = taxOutput.NationalInsurance.ToString("C");
            lblTotalDeductions.Text = taxOutput.TotalDeductions.ToString("C");
            lblNetSalary.Text = taxOutput.NetSalary.ToString("C");

            //if user says they are paying more pension than they get in salary
            if ((taxOutput.NetSalary == 0) && (taxOutput.PensionAnnualAmount > 0))
            {
                lblPensionError.Text = "Aren't your pension contributions a bit too high?";
            }
            double totalTaxAndNiPaid = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate
                                + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance;

            taxOutput.PensionsContribution = totalTaxAndNiPaid * pensionsSpending;
            taxOutput.HealthContribution = totalTaxAndNiPaid * healthSpending;
            taxOutput.EducationContribution = totalTaxAndNiPaid * educationSpending;
            taxOutput.DefenceContribution = totalTaxAndNiPaid * defenceSpending;
            taxOutput.WelfareContribution = totalTaxAndNiPaid * welfareSpending;
            taxOutput.ProtectionContribution = totalTaxAndNiPaid * protectionSpending;
            taxOutput.TransportContribution = totalTaxAndNiPaid * transportSpending;
            taxOutput.GeneralContribution = totalTaxAndNiPaid * generalSpending;
            taxOutput.OtherContribution = totalTaxAndNiPaid * otherSpending;
            taxOutput.InterestContribution = totalTaxAndNiPaid * interestSpending;

            //DrawCharts(taxOutput);
            //DrawTaxGraph(taxOutput);
            //DrawContributionsGraph(taxOutput);
            DrawBothCharts(taxOutput);
        }
}

    protected void tbPension_TextChanged(object sender, EventArgs e)
    {
        
    }

    protected void ddlStudentLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    //static methods for calculations--------------------------------------------------------------------
    static TaxOutput CalculateTax(double grossSalaryInput, double childcareVoucherMonthlyAmount, double pensionAnnualAmount, double studentLoanAnnualPayment)
    {
        TaxOutput taxOutput = new TaxOutput();

        double grossSalaryForCalculation = 0;
        //deduct childcare vouchers and pension contributions before starting tax calculation
        double childcareVoucherAnnualAmount = childcareVoucherMonthlyAmount * 12;
        grossSalaryForCalculation = grossSalaryInput - childcareVoucherAnnualAmount - pensionAnnualAmount;

        taxOutput.PersonalAllowance = CalculatePersonalAllowance(grossSalaryForCalculation);
        taxOutput.TaxBasicRate = CalculateBasicRateTax(grossSalaryForCalculation);
        taxOutput.TaxHigherRate = CalculateHigherRateTax(grossSalaryForCalculation, taxOutput.PersonalAllowance);
        taxOutput.TaxAdditionalRate = CalculateAdditionalRateTax(grossSalaryForCalculation);
        taxOutput.NationalInsurance = CalculateNationalInsurance(grossSalaryForCalculation);
        taxOutput.TotalDeductions = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance + childcareVoucherAnnualAmount + pensionAnnualAmount + studentLoanAnnualPayment;
        taxOutput.NetSalary = grossSalaryInput - taxOutput.TotalDeductions;
        taxOutput.ChildcareVoucherAmount = childcareVoucherAnnualAmount;
        taxOutput.PensionAnnualAmount = pensionAnnualAmount;
        taxOutput.GrossSalaryInput = grossSalaryInput;
        taxOutput.StudentLoanAnnualDeduction = studentLoanAnnualPayment;

        //add contributions

        //if deductions exceed income
        if (taxOutput.NetSalary < 0)
        {
            taxOutput.NetSalary = 0;
            taxOutput.TotalDeductions = grossSalaryInput;
            pensionAnnualAmount = grossSalaryInput - taxOutput.TaxBasicRate - taxOutput.TaxHigherRate - taxOutput.TaxAdditionalRate - taxOutput.NationalInsurance - childcareVoucherAnnualAmount - studentLoanAnnualPayment;
        }
        return taxOutput;
    }

    static double CalculatePersonalAllowance(double grossSalary)
    {
        double personalAllowance = 0;

        //adjust personal allowance downwards for highest earners
        if (grossSalary > personalAllowanceAdjustmentThreshold)
        {
            personalAllowance = personalAllowanceLimit - ((grossSalary - personalAllowanceAdjustmentThreshold) / 2);
            if (personalAllowance < 0)
            {
                personalAllowance = 0;
            }
        }
        else  //don't change personal allowance
        {
            personalAllowance = personalAllowanceLimit;
        }

        return personalAllowance;
    }

    static double CalculateBasicRateTax(double grossSalary)
    {
        double taxBasicRate = 0;
        //if the maximum basic rate tax is paid ...
        if (grossSalary > higherRateThreshold)
        {
            taxBasicRate = (higherRateThreshold - personalAllowanceLimit) * basicRate;
        }
        //if less than the maximum basic rate tax is paid AND the user earns
        //more than the personal allowance
        else if (grossSalary > personalAllowanceLimit)
        {
            taxBasicRate = (grossSalary - personalAllowanceLimit) * basicRate;
        }

        return taxBasicRate;
    }

    static double CalculateHigherRateTax(double grossSalary, double personalAllowance)
    {
        double taxHigherRate = 0;
        //higher rate tax may involve alterations to personal allowance 
        double personalAllowanceDeduction = personalAllowanceLimit - personalAllowance;

        if (grossSalary > higherRateThreshold)
        {
            if (grossSalary > additionalRateThreshold)
            {
                taxHigherRate = (additionalRateThreshold - higherRateThreshold + personalAllowanceDeduction) * higherRate;
            }
            else
            {
                taxHigherRate = (grossSalary - higherRateThreshold + personalAllowanceDeduction) * higherRate;
            }

        }

        return taxHigherRate;
    }

    static double CalculateAdditionalRateTax(double grossSalary)
    {
        double taxAdditionalRate = 0;

        if (grossSalary > additionalRateThreshold)
        {
            taxAdditionalRate = (grossSalary - additionalRateThreshold) * additionalRate;
        }

        return taxAdditionalRate;
    }

    static double CalculateNationalInsurance(double grossSalary)
    {
        double nationalInsurancePaid = 0;

        if (grossSalary > nationalInsuranceThreshold)
        {
            if (grossSalary > nationalInsuranceUpperEarningsLimit)
            {
                nationalInsurancePaid = (nationalInsuranceUpperEarningsLimit - nationalInsuranceThreshold) * 0.12
                    + (grossSalary - nationalInsuranceUpperEarningsLimit) * 0.02;
            }
            else
            {
                nationalInsurancePaid = (grossSalary - nationalInsuranceThreshold) * 0.12;
            }
        }

        return nationalInsurancePaid;
    }

    static MethodReturn CheckChildCareVoucherAmount(double grossSalaryInput, double childcareVoucherMonthlyAmount)
    {
        MethodReturn methodReturn = new MethodReturn(0,"");

        //ensure childcare voucher amount doesn't exceed maximum allowed
        if ((childcareVoucherMonthlyAmount > basicRateMaximumChildcareVouchers) && (grossSalaryInput <= higherRateThreshold))
        {
            methodReturn.errorMessage = "Your monthly childcare voucher has been set to £" + basicRateMaximumChildcareVouchers + " which is the maximum permitted.";
            methodReturn.returnValue = basicRateMaximumChildcareVouchers;
        }
        if ((childcareVoucherMonthlyAmount > higherRateMaximumChildcareVouchers) && (grossSalaryInput > higherRateThreshold) && (grossSalaryInput <= additionalRateThreshold))
        {
            methodReturn.errorMessage = "Your monthly childcare voucher has been set to £" + higherRateMaximumChildcareVouchers + " which is the maximum permitted.";
            methodReturn.returnValue = higherRateMaximumChildcareVouchers;
        }
        if ((childcareVoucherMonthlyAmount > additionalRateMaximumChildcareVouchers) && (grossSalaryInput > additionalRateThreshold))
        {
            methodReturn.errorMessage = "Your monthly childcare voucher has been set to £" + additionalRateMaximumChildcareVouchers + " which is the maximum permitted.";
            methodReturn.returnValue = additionalRateMaximumChildcareVouchers;
        }

        return methodReturn;
    }

    static string CalculateChildcareVoucherDeduction(double grossSalaryInput, double childcareVoucherAmount, bool validInput)
    {
        //global variable childcareVoucherAmount is set here

        if (!validInput || childcareVoucherAmount < 0)
        {
            childcareVoucherAmount = 0;
            validInput = true;
            return "Please enter a zero or a positive number for childcare voucher amount";
        }
        
        

        childcareVoucherAmount *= 12;

        return "";
    }

    static MethodReturn CheckPensionInputAndCalculate(string pensionEntered, double grossSalary)
    {
        MethodReturn methodReturn = new MethodReturn(0, "");
        double pensionPercentageAmount = 0;

        //if the field is empty
        if(String.IsNullOrEmpty(pensionEntered))
        {
            pensionEntered = "0";
        }
        //if a percentage has been entered for pension contribution
        if ((pensionEntered).Contains("%"))
        {
            string[] pensionPercentage = (pensionEntered).Split('%');

            validInput = Double.TryParse(pensionPercentage[0], out pensionPercentageAmount);
            if (!validInput || (pensionPercentageAmount <= 0) || (pensionPercentageAmount >= 100))
            {
                pensionPercentageAmount = 0;
                methodReturn.returnValue = 0;
                methodReturn.errorMessage = "Please enter a valid amount or percentage for pension contributions";
            }
            else
            {
                methodReturn.errorMessage = "";
                methodReturn.returnValue  = grossSalary * (pensionPercentageAmount / 100);
            }
        }
        else //if an amount has been entered for pension contribution
        {
            double pensionMonthlyAmount = 0;
            validInput = Double.TryParse(pensionEntered, out pensionMonthlyAmount);
            if (!validInput || pensionMonthlyAmount < 0)
            {
                methodReturn.errorMessage = "Please enter zero or a positive amount or percentage for pension contribution";
                methodReturn.returnValue = 0;
            }
            else
            {
                methodReturn.errorMessage = "";
                methodReturn.returnValue = pensionMonthlyAmount * 12;
            }
        }

        return methodReturn;
    }

    static MethodReturn CalculateStudentLoanPayment(double grossSalary, string selectedOption)
    {
        double studentLoanAnnualDeduction = 0;
        MethodReturn methodReturn = new MethodReturn();

        if (selectedOption == "Plan1" && grossSalary >= studentLoanPlan1Threshold)
        {
            studentLoanAnnualDeduction = (grossSalary - studentLoanPlan1Threshold) * studentLoanRepaymentRate;
        }
        else if (selectedOption == "Plan2" && grossSalary >= studentLoanPlan2Threshold)
        {
            studentLoanAnnualDeduction = (grossSalary - studentLoanPlan2Threshold) * studentLoanRepaymentRate;
        }
        else
        {
            studentLoanAnnualDeduction = 0;
        }

        methodReturn.returnValue = studentLoanAnnualDeduction;

        return methodReturn;
    }


//----Charts-------------------------------------------------------------------------
    void DrawCharts(TaxOutput taxOutput)
    {
        hldataSource.Visible = true;

        double totalTaxAndNiPaid = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate
                                 + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance;

        double pensionsContribution = totalTaxAndNiPaid * pensionsSpending;
        double healthContribution = totalTaxAndNiPaid * healthSpending;
        double educationContribution = totalTaxAndNiPaid * educationSpending;
        double defenceContribution = totalTaxAndNiPaid * defenceSpending;
        double welfareContribution = totalTaxAndNiPaid * welfareSpending;
        double protectionContribution = totalTaxAndNiPaid * protectionSpending;
        double transportContribution = totalTaxAndNiPaid * transportSpending;
        double generalContribution = totalTaxAndNiPaid * generalSpending;
        double otherContribution = totalTaxAndNiPaid * otherSpending;
        double interestContribution = totalTaxAndNiPaid * interestSpending;

        ClientScript.RegisterStartupScript(GetType(), "draw", "drawContributions('" + taxOutput.PersonalAllowance + "','"
                                                                       + taxOutput.TaxBasicRate + "','"
                                                                       + taxOutput.TaxHigherRate + "','"
                                                                       + taxOutput.TaxAdditionalRate + "','"
                                                                       + taxOutput.NationalInsurance + "','"
                                                                       + taxOutput.NetSalary + "','"
                                                                       + taxOutput.ChildcareVoucherAmount + "','"
                                                                       + taxOutput.StudentLoanAnnualDeduction + "','"
                                                                       + taxOutput.PensionAnnualAmount + "','"
                                                                       + taxOutput.GrossSalaryInput + "','"
                                                                       + pensionsContribution + "','"
                                                                       + healthContribution + "','"
                                                                       + educationContribution + "','"
                                                                       + defenceContribution + "','"
                                                                       + welfareContribution + "','"
                                                                       + protectionContribution + "','"
                                                                       + transportContribution + "','"
                                                                       + generalContribution + "','"
                                                                       + otherContribution + "','"
                                                                       + interestContribution + "','"
                                                                       + totalTaxAndNiPaid + "');", true);
    }

    void DrawTaxGraph(TaxOutput taxOutput)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "anything", "drawTax('" + taxOutput.NetSalary + "','" 
                                                                                + taxOutput.TaxBasicRate + "','"
                                                                                + taxOutput.TaxHigherRate + "','"
                                                                                + taxOutput.TaxAdditionalRate + "','"
                                                                                + taxOutput.NationalInsurance + "','"
                                                                                + taxOutput.PensionAnnualAmount + "','"
                                                                                + taxOutput.ChildcareVoucherAmount + "');", true);
    }

    void DrawContributionsGraph(TaxOutput taxOutput)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "anything", "drawContributions('" + taxOutput.PensionsContribution + "','"
                                                                       + taxOutput.HealthContribution + "','"
                                                                       + taxOutput.EducationContribution + "','"
                                                                       + taxOutput.DefenceContribution + "','"
                                                                       + taxOutput.WelfareContribution + "','"
                                                                       + taxOutput.ProtectionContribution + "','"
                                                                       + taxOutput.TransportContribution + "','"
                                                                       + taxOutput.GeneralContribution + "','"
                                                                       + taxOutput.OtherContribution + "','"
                                                                       + taxOutput.InterestContribution + "');", true);

    }


    void DrawBothCharts(TaxOutput taxOutput)
    {
        hldataSource.Visible = true;

        ClientScript.RegisterStartupScript(this.GetType(), "anything", "draw('" + taxOutput.NetSalary + "','"
                                                                                + taxOutput.TaxBasicRate + "','"
                                                                                + taxOutput.TaxHigherRate + "','"
                                                                                + taxOutput.TaxAdditionalRate + "','"
                                                                                + taxOutput.NationalInsurance + "','"
                                                                                + taxOutput.PensionAnnualAmount + "','"
                                                                                + taxOutput.ChildcareVoucherAmount + "','"
                                                                                + taxOutput.StudentLoanAnnualDeduction + "','"
                                                                                + taxOutput.PensionsContribution + "','"
                                                                                + taxOutput.HealthContribution + "','"
                                                                                + taxOutput.EducationContribution + "','"
                                                                                + taxOutput.DefenceContribution + "','"
                                                                                + taxOutput.WelfareContribution + "','"
                                                                                + taxOutput.ProtectionContribution + "','"
                                                                                + taxOutput.TransportContribution + "','"
                                                                                + taxOutput.GeneralContribution + "','"
                                                                                + taxOutput.OtherContribution + "','"
                                                                                + taxOutput.InterestContribution
                                                                                + "');", true);
    }
}