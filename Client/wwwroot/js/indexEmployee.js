

$(document).ready(() => {

    SetstatisticEmployee()
})


function SetstatisticEmployee() {
    $.ajax({
        url: `https://localhost:44302/api/leavingrequest/emp/${idEmp}/statistic`
    }).done(res => {
        $(".waitEmp").html(res.totalMenunggu)
        $(".approveEmp").html(res.totalApprove)
        $(".rejEmp").html(res.totalReject)
        $(".revEmp").html(res.totalRevisi)

        var options = {
            chart: {
                height:260,
                type: 'bar',
            },
           
            series: [
                {
                    name: "Total",
                    data: [res.totalMenunggu, res.totalApprove, res.totalReject, res.totalRevisi]
                }
            ],
            plotOptions: {
                bar: {
                    distributed: true, // this line is mandatory
                   
                },
            },
           
            colors: ['#0275d8', '#5cb85c', '#d9534f', '#f0ad4e'],
           
            xaxis: {
                categories:["Waiting","Approve", "Reject", "Revisi"]
            },
            legend: {
                show: true,
                position:"top"
            }



        }

        var chart = new ApexCharts(document.querySelector("#statistic"), options);

        chart.render();
    })

}

