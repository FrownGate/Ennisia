// Define the Cloud Script function that returns the current date and time
handlers.GetServerDateTime = function (args, context) {
    var currentTime = new Date();
    return { Result: "Success", ServerDateTime: currentTime.toISOString() };
};
