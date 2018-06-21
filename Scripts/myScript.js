//draws 2 charts using CanvasJS plugin.  Based on CanvasJS documentation

draw = function (personalAllowance, taxBasicRate, taxHigherRate, taxAdditionalRate,
                nationalInsurance, netSalary, childcareVoucherAmount, studentLoanAnnualDeduction,
                pensionAnnualAmount, grossSalaryInput,
                pensionsContribution, healthContribution, educationContribution, defenceContribution,
                welfareContribution, protectionContribution, transportContribution, generalContribution,
                otherContribution, interestContribution, totalTaxAndNiPaid) {
    //if there isn't enough data, make all HTML elements associated with the charts invisible
    if (personalAllowance === "0" && taxBasicRate === "0" && nationalInsurance === "0") {
        document.getElementById("chartPlaceholder1").style.display = "none";
        document.getElementById("noDataPlaceholder1").textContent = "Not enough data to draw chart";
        document.getElementById("chartPlaceholder2").style.display = "none";
        document.getElementById("noDataPlaceholder2").textContent = "Not enough data to draw chart";
    }
    else {
        //draw the tax chart, showing taxes and other deductions and net salary
        var chart = new CanvasJS.Chart("chartContainer1",
        {
            //theme: "theme2",
            title: {
                text: "Proportion of £" + grossSalaryInput + " salary paid in tax and NI",
                fontFamily: "tahoma"
            },
            animationEnabled: true,
            animationDuration: 1000,
            data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} £{y} - #percent %",
                yValueFormatString: "#,###.##",
                legendText: "{indexLabel}",
                dataPoints: [
                    { y: parseFloat(netSalary), indexLabel: "Net salary" },
                    { y: parseFloat(taxBasicRate), indexLabel: "Basic rate tax" },
                    { y: parseFloat(taxHigherRate), indexLabel: "Higher rate tax" },
                    { y: parseFloat(taxAdditionalRate), indexLabel: "Additional rate tax"},
                    { y: parseFloat(nationalInsurance), indexLabel: "National insurance" },
                    { y: parseFloat(childcareVoucherAmount), indexLabel: "Childcare vouchers" },
                    { y: parseFloat(studentLoanAnnualDeduction), indexLabel: "Student loan repayment" },
                    { y: parseFloat(pensionAnnualAmount), indexLabel: "Pension contribution" }
                ]
            }
            ]
        });
        chart.render();
        //draw contributions chart

        var chart = new CanvasJS.Chart("chartContainer2",
        {
            //theme: "theme2",
            title: {
                text: "Contribution to government spending out of £" + totalTaxAndNiPaid,
                fontFamily: "tahoma"
            },
            subtitles: [{ text: "(Total tax and NI paid)", fontSize: 24}],
            animationEnabled: true,
            animationDuration: 1000,
            data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} £{y} - #percent %",
                yValueFormatString: "#,###.00",
                legendText: "{indexLabel}",
                dataPoints: [
                    { y: parseFloat(pensionsContribution), indexLabel: "Pensions" },
                    { y: parseFloat(healthContribution), indexLabel: "Health" },
                    { y: parseFloat(educationContribution), indexLabel: "Education" },
                    { y: parseFloat(defenceContribution), indexLabel: "Defence" },
                    { y: parseFloat(welfareContribution), indexLabel: "Welfare" },
                    { y: parseFloat(protectionContribution), indexLabel: "Police, Fire, Law" },
                    { y: parseFloat(transportContribution), indexLabel: "Transport" },
                    { y: parseFloat(generalContribution), indexLabel: "General" },
                    { y: parseFloat(otherContribution), indexLabel: "Other (Agriculture, Industry, Environment, etc.)" },
                    { y: parseFloat(interestContribution), indexLabel: "Interest" }
                ]
            }
            ]
        });
        chart.render();
    }
}
