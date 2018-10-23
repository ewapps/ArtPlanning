// Var for session timeout
var msecPageLoad;
var msecTimeOut;
var msecWarning;
var mySessionTimeOut;
// Local for traductions
var pendingTitle = "";
var pendingConfirm = "";
var pendingText = "Vous êtes resté inactif, votre session va expirer dans #1";
var redirectTitle = "";
var redirectConfirm = "";
var redirectText = "";
// urls
var timeoutVal = 0;
var keepAliveUrl = "";
var logoffUrl = "";
var timerLogoff;

function initPendingTraductions(pTitle, pText, pConfirm) {
    pendingTitle = pTitle;
    pendingText = pText;
    pendingConfirm = pConfirm;
}
function initRedirectTraductions(rTitle, rText, rConfirm) {
    redirectTitle = rTitle;
    redirectText = rText;
    redirectConfirm = rConfirm;
}
function initTimeoutValues(toutVal, aliveUrl, offUrl) {
    timeoutVal = toutVal;
    keepAliveUrl = aliveUrl;
    logoffUrl = offUrl;
    SessionTimerInit();
}

// Reset timeout session popup
function ResetTimer() {
    clearTimeout(mySessionTimeOut);
    SessionTimerInit();
}
// Init timeout session popup
function SessionTimerInit() {
    SetPageTimes();
    mySessionTimeOut = setTimeout("ShowPendingTimeoutDialog()", msecWarning);
}
// Init var for session timeout popup
function SetPageTimes() {
    msecPageLoad = new Date().getTime();
    msecTimeOut = (timeoutVal * 60 * 1000);
    msecWarning = (timeoutVal - 1) * 60 * 1000; // alert one minute before end of session
}
// Display timeout alert popup
function ShowPendingTimeoutDialog() {
    UpdateTimeoutMessage();
    swal({
        title: pendingTitle,
        text: pendingText.replace(/#1/, 0),
        type: "warning",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: pendingConfirm,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {
        ResetTimeout();
    });
}
// Update timeout counter in popup
function UpdateTimeoutMessage() {
    var msecElapsed = (new Date().getTime()) - msecPageLoad;
    var timeLeft = msecTimeOut - msecElapsed; //time left in miliseconds
    if (timeLeft <= 0) {
        RedirectToWelcomePage();
    } else {
        var minutesLeft = Math.floor(timeLeft / 60000);
        var secondsLeft = Math.floor((timeLeft % 60000) / 1000);
        var sMinutesLeft = ("00" + (minutesLeft).toString()).slice(-2) + ":";
        var sSecondsLeft = ("00" + (secondsLeft).toString()).slice(-2);
        $('.swal2-content').text(pendingText.replace(/#1/, "" + sMinutesLeft + sSecondsLeft + ""));
        timerLogoff = setTimeout("UpdateTimeoutMessage()", 50);
    }
}
// Reset session to keep alive
function ResetTimeout() {
    $.ajax({
        url: keepAliveUrl
    });
    clearTimeout(timerLogoff);
    SessionTimerInit();
}
// Session expired, redirect to login
function RedirectToWelcomePage() {
    window.location = logoffUrl;
    //swal({
    //    title: redirectTitle,
    //    text: redirectText,
    //    type: "error",
    //    confirmButtonColor: "#DD6B55",
    //    confirmButtonText: redirectConfirm,
    //    allowOutsideClick: false,
    //    allowEscapeKey: false
    //}).then(function () {
    //    window.location = logoffUrl;
    //}, function (dismiss) {
    //    window.location = logoffUrl;
    //})
}