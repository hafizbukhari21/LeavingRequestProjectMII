const idEmp = $("#login-employee-id").val()
let accReqForm = document.getElementsByClassName("accReqForm");


$(document).ready(function () {
    
    let man = {
        manager_id: "Employee0007"
    }

    $('#dataTbl').DataTable({
        ajax: {
            url: `https://localhost:44302/api/leavingrequest/man/${idEmp}`,
            //url: `https://localhost:44302/api/leavingrequest/man/Employee0007`,
            "dataType": "json",
            "dataSrc": "",
        },
        "order": [[0, "desc"]],
        dom: 'lBfrtip',
        buttons: [
            //'copy', 'csv', 'excel', 'pdf', 'print'
            {
                extend: 'copy',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-copy"> </i>',
                titleAttr: 'Copy to clipboard'
            },
            {
                extend: 'excel',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-excel"> </i>',
                titleAttr: 'Download to excel'
            },
            {
                extend: 'pdf',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-pdf"> </i>',
                titleAttr: 'Download to pdf'
            },
            {
                extend: 'print',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-print"> </i>',
                titleAttr: 'Print this table'
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
        },
        columns: [
            {
                "data": "requestTime",
                visible: false,
            },
            {
                "data": "employeeName"
            },
            {
                "data": "categoryName"
            },
            {
                "data": "approvalStatus",
                "render": function (data, type, row, meta) {
                    let badge = ""
                    switch (data) {
                        case "Menunggu":
                            badge = "primary"
                            break;
                        case "Ditolak":
                            badge = "danger"
                            break
                        case "Diterima":
                            badge = "success"
                            break;
                        case "Revisi":
                            badge = "warning"
                            break;
                        case "Cancel":
                            badge = "secondary"
                            break;
                    }
                    return `<span class="badge badge-${badge}">${data}</span>`
                }
            },
            {
                data: "request_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <button type="button" class="btn btn-sm btn-warning" title="Delete" onClick="leaveDetail('${data}')" data-toggle="modal" data-target="#updateModal">
                                <i class="fas fa-edit"></i> 
                                </button>
                                
                            </div>`
                }
            }
        ]
    });
});

function leaveDetail(reqId) {
    let data = {
        request_id: reqId
    }
    console.log(data)
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/emp/detail",
        data: JSON.stringify(data),
        type: "POST",
        contentType: 'application/json',
    }).done(u => {
        $("#request_ids").val(u.request_id);
        $("#t-request_id").html(u.request_id);
        $("#employee_id").val(u.employee_id);
        $("#category_id").val(u.category_name);
        $("#startDate").val(moment(u.startDate).format("YYYY-MM-DD").toString());
        $("#endDate").val(moment(u.endDate).format("YYYY-MM-DD").toString());
        //$("#approvalStatusName").val(u.approvalStatusName);
        $("#leavingMessage").val(u.leavingMessage);
        $("#downloadFileBukti").attr("href", "data:application/octet-stream;base64,"+ u.fileBukti);
        $("#downloadFileBukti").attr("download", u.namaFileBukti);
        $("#approvalMessage").val(u.approvalMessage)
        console.log(u)
        let modalUpdateFooter = document.querySelector("#modalFooterUpdate")
        if (u.approvalStatus == 1 || u.approvalStatus == 2 || u.approvalStatus == 4) {
            modalUpdateFooter.style.display = "none";
        }
        else {
            modalUpdateFooter.style.display = "flex";
        }
        switch (u.approvalStatusName) {
            case "Menunggu":
                document.getElementById("t-approvalStatusName").innerHTML = u.approvalStatusName;
                document.getElementById("t-approvalStatusName").className = "modal-title badge badge-primary";
                break
            case "Diterima":
                document.getElementById("t-approvalStatusName").innerHTML = u.approvalStatusName;
                document.getElementById("t-approvalStatusName").className = "modal-title badge badge-success";
                break
            case "Ditolak":
                document.getElementById("t-approvalStatusName").innerHTML = u.approvalStatusName;
                document.getElementById("t-approvalStatusName").className = "modal-title badge badge-danger";
                break
            case "Revisi":
                document.getElementById("t-approvalStatusName").innerHTML = u.approvalStatusName;
                document.getElementById("t-approvalStatusName").className = "modal-title badge badge-warning";
                break
            default:
                document.getElementById("t-approvalStatusName").innerHTML = u.approvalStatusName;
                document.getElementById("t-approvalStatusName").className = "modal-title badge badge-secondary";
                break
        }
    })
}

function miniVal(accReqForm) {
    var validation = Array.prototype.filter.call(accReqForm, function (form) {
        form.addEventListener('submit', function (event) {
            event.preventDefault();
            event.stopPropagation();
            
            
            form.classList.add('was-validated');
        }, false);
    });
    }


function ApproveReq() {
    Swal.fire({
        title: 'Please Wait !!!',
        html: `<div class="spinner-grow text-primary" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-secondary" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-success" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-danger" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-warning" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>`,
        allowOutsideClick: false,
        onBeforeOpen: () => {
            Swal.showLoading()
        },
        showConfirmButton: false,
    });
    let approvalMessage = $("#approvalMessage").val();
    if (approvalMessage == "" || approvalMessage == null) {
        miniVal(accReqForm)
        return
    }
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    obj.approvalMessage = approvalMessage 
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/approve",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        if (result.errorType == 200) {
            let modalUpdateFooter = document.querySelector("#modalFooterUpdate")
            modalUpdateFooter.style.display = "none";
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: result.message,
            })
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'fail',
                text: result.message,
            })
        }
        
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}

function RejectReq() {
    let approvalMessage = $("#approvalMessage").val();
    if (approvalMessage == "" || approvalMessage == null) {
        miniVal(accReqForm)
        return
    }
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    obj.approvalMessage = approvalMessage 
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/reject",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        let modalUpdateFooter = document.querySelector("#modalFooterUpdate")
        modalUpdateFooter.style.display = "none";
        console.log(result)
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,
        })
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}

function RevisionReq() {
    let approvalMessage = $("#approvalMessage").val();
    if (approvalMessage == "" || approvalMessage == null) {
        miniVal(accReqForm)
        return
    }
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    obj.approvalMessage = approvalMessage 
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/revisi",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,   
        })
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}