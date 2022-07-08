$(document).ready(() => {
    let forgotPassForm = document.getElementsByClassName("formForgotPass")
    ValidateForm(forgotPassForm, doForgotPassword)
})

function doForgotPassword() {
    let data = {
        email:  document.querySelector("#LoginEmail").value
    }
    $.ajax({
        url: "https://localhost:44302/api/employee/forgotPassword",
        data: JSON.stringify(data),
        type: "PATCH",
        contentType:"application/json"
    }).done(res => {
        switch (res.errorType) {
            case 200:
                SwallSuccess(res.message)
                sessionStorage.setItem("emailForgot", data.email)
                setTimeout(() => {
                    InsertOtpModal()
                }, 1500)
                break
            default:
                SwallFail(res.message)
        }
    }).fail(() => {
        SwallFail(res.message)
    })
}  


function InsertOtpModal() {

    Swal.fire({
        title: 'Insert Your OTP And new Password',
        html:
           `        <div class="form-outline mb-4">
                        <input type="text" id="otp" class="form-control" autocomplete="off"
                               placeholder="Input your OTP" />
                    </div>

                    <!-- Password input -->
                    <div class="form-outline mb-3">
                        <input type="password" id="inputforgot" class="form-control" autocomplete="off"
                               placeholder="Enter New password" />
                   </div>
                    <!-- Password input -->
                    <div class="form-outline mb-3">
                        <input type="password" id="validateForgot" class="form-control" autocomplete="off"
                               placeholder="Enter Confirm New password" />
                   </div>
                    `,
        
        inputAttributes: {
            autocapitalize: 'off',
        },
        showCancelButton: true,
        confirmButtonText: 'Look up',
        showLoaderOnConfirm: true,
        preConfirm: () => {

            let inputForgot = document.querySelector("#inputforgot").value
            let validateForgot = document.querySelector("#validateForgot").value
            let otp = document.querySelector("#otp").value
            if (inputForgot !== validateForgot) {
                Swal.showValidationMessage("Password Baru tidak Match")
            }
            else {
                let data = {
                    email: sessionStorage.getItem("emailForgot"),
                    tokenOtp: otp,
                    password: inputForgot
                }
               return  AjaxwithOTP(data)
            }

            function AjaxwithOTP(data) {
                $.ajax({
                    url: "https://localhost:44302/api/employee/forgotPassword/validate",
                    type: "PATCH",
                    contentType: "application/json",
                    data: JSON.stringify(data)
                }).done(res => {
                    if (res.errorType == 200) SwallSuccess(res.message)
                    else {
                        SwallFail(res.message)
                        setTimeout(() => { InsertOtpModal()},1500)
                    } 
                })
            }
            
            

            //return fetch(`//api.github.com/users/${login}`)
            //    .then(response => {
            //        if (!response.ok) {
            //            throw new Error(response.statusText)
            //        }
            //        return response.json()
            //    })
            //    .catch(error => {
            //        Swal.showValidationMessage(
            //            `Request failed: ${error}`
            //        )
            //    })
        },
        allowOutsideClick:false
    }).then((result) => {
        console.log({ data: result})
        //if (result.errorType == 200) {
        //    SwallSuccess(result.message)
        //} else {
        //    SwallFail(result.message)
        //}
    })
}



