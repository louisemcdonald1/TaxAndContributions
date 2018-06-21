draw = function (netSalary, tax20, tax40, tax45, nationalInsurance, pension, ccVouchers, studentLoan, PensionsContribution, HealthContribution, EducationContribution, DefenceContribution, WelfareContribution, ProtectionContribution, TransportContribution, GeneralContribution, OtherContribution, InterestContribution) {

    var taxData = [parseFloat(netSalary), parseFloat(tax20), parseFloat(tax40), parseFloat(tax45), parseFloat(nationalInsurance), parseFloat(pension), parseFloat(ccVouchers), parseFloat(studentLoan)]
    //access chart element
    const ctx1 = document.getElementById("taxChart");
    //var taxData = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    var taxLabels = ["Net salary", "Tax at 20%", "Tax at 40%", "Tax at 45%", "National Insurance", "Pension contribution", "Childcare vouchers", "Student loan repayment"];
    //set up data
    data = {
        datasets: [{
            data: taxData,
            backgroundColor: [
                   'rgba(0, 0, 255, 0.5)',
                   'rgba(255, 0, 0, 0.2)',
                   'rgba(255, 0, 0, 0.5)',
                   'rgba(255, 0, 0, 0.8)',
                   'rgba(255, 255, 0, 0.5)',
                   'rgba(255, 255, 255, 0.5)',
                   'rgba(120, 0, 255, 0.5)',
                   'rgba(255, 0, 104, 0.5)']
        }],

        // These labels appear in the legend and in the tooltips when hovering different arcs
        labels: taxLabels


    };

    //draw chart
    var taxChart = new Chart(ctx1, {
        type: 'pie',
        data: data,
        options: {
            title: {
                display: true,
                fontFamily: "'Francois One','Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                fontSize: 18,
                fontStyle: 'bold',
                text: 'Net salary, tax and other contributions'
            },
            legend: {
                display: false,
                labels: {
                    fontColor: 'rgb(255, 99, 132)'
                }
            }
        }
    });

    var contributionData = [parseFloat(PensionsContribution), parseFloat(HealthContribution), parseFloat(EducationContribution), parseFloat(DefenceContribution), parseFloat(WelfareContribution), parseFloat(ProtectionContribution), parseFloat(TransportContribution), parseFloat(GeneralContribution), parseFloat(OtherContribution), parseFloat(InterestContribution)]
    //access chart element
    const ctx2 = document.getElementById("contributionsChart");
    //var taxData = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    var contributionLabels = ["Pensions", "Health", "Education", "Defence", "Welfare", "Police, Law, Justice", "Transport", "General", "Other", "Interest"];
    //set up data
    data = {
        datasets: [{
            data: contributionData,
            backgroundColor: [
                   'rgba(0, 0, 255, 0.5)',
                   'rgba(0, 255, 0, 0.5)',
                   'rgba(255, 102, 0, 0.5)',
                   'rgba(255, 0, 0, 0.8)',
                   'rgba(0, 0, 255, 0.8)',
                   'rgba(255, 255, 0, 0.5)',
                   'rgba(255, 255, 255, 0.5)',
                   'rgba(120, 0, 255, 0.5)',
                   'rgba(255, 0, 104, 0.5)',
                   'rgba(0, 0, 0, 0.5)']
        }],

        // These labels appear in the legend and in the tooltips when hovering different arcs
        labels: contributionLabels


    };

    //draw chart
    var contributionsChart = new Chart(ctx2, {
        type: 'pie',
        data: data,
        options: {
            title: {
                display: true,
                fontFamily: "'Francois One','Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                fontSize: 18,
                fontStyle: 'bold',
                text: 'Contribution to goverment spending'
            },
            legend: {
                display: false,
                labels: {
                    fontColor: 'rgb(255, 99, 132)'
                }
            }
        }
    });
}
