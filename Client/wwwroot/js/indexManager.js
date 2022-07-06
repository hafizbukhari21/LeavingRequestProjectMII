
$(document).ready(() => {
    GetEmpWitTotLibur()
    SetstatisticManager()
})

function GetEmpWitTotLibur() {
    
    $.ajax({
        url: `https://localhost:44302/api/employee/man/${idEmp}/empWithTotCuti`
    }).done((res) => {
        res.forEach(e => {
            $("#listSisaCutiEmp").append(
                `<li class="list-group-item">
                    <div class="widget-content p-0">
                        <div class="widget-content-wrapper">
                            <div class="widget-content-left mr-3">
                                <img width="42" class="rounded-circle" src="assets/images/avatars/5.jpg" alt="">
                            </div>
                            <div class="widget-content-left">
                                <div class="widget-heading">${e.name}</div>
                                <div class="widget-subheading">${e.namaDivisi}</div>
                            </div>

                            <div class="widget-content-right w-50">
                                <div class="progress-bar-xs progress">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${e.sisaCuti}" aria-valuemin="0" aria-valuemax="12" style="width: ${parseInt(e.sisaCuti)/12*100}%;">${e.sisaCuti}</div>
                                </div>
                            </div>

                        </div>
                    </div>
                </li>`
            )
        })
    })
}

function SetstatisticManager() {
    $.ajax({
        url: `https://localhost:44302/api/leavingrequest/man/${idEmp}/statistic`
    }).done(res => {
        $(".waitMan").html(res.totalMenunggu)
        $(".approveMan").html(res.totalApprove)
        $(".rejMan").html(res.totalReject)
        $(".revMan").html(res.totalRevisi)
    })

}