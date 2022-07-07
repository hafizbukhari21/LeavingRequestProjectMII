

$(document).ready(() => {

    SetstatisticEmployee()
})
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
    })

}