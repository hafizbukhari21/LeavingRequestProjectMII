//countdown back page
ProgressCountdown(5, 'pageBeginCountdown', 'pageBeginCountdownText');
function ProgressCountdown(timeleft, bar, text) {
    return new Promise((resolve, reject) => {
        var countdownTimer = setInterval(() => {
            timeleft--;

            document.getElementById(bar).value = timeleft;
            document.getElementById(text).textContent = timeleft;

            if (timeleft <= 0) {
                window.location.replace("../Login")
                clearInterval(countdownTimer);
                resolve(true);
            }
        }, 1000);
    });
}