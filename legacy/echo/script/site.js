function isReady()
{
	registerForValidation();

	$('.close a').click(function() { $($(this).attr('rel')).hide(); });
	$('a[title]').mouseover(function()
	{
		window.status = $(this).attr('title');
	}).mouseout(function()
	{
		window.status = window.defaultStatus;
	});

	if (typeof (pageInit) == "function")
		pageInit();
}

function registerForValidation() {

    var options = { onblur: true, focusInvalid: true, ignore: ":hidden", errorPlacement: echoErrorPlacement, onSubmit: false };

    $('#aspnetForm').validate(options);
}

function echoErrorPlacement(error, element) 
{
	var elementID = element.attr('id');
	var labelSelector = 'label[for=' + elementID + ']';
    var errorText = error.attr('innerHTML');

    $(labelSelector).each(function() { if (element.hasClass('date') || element.hasClass('number')) { errorText = errorText.replace('.', ' for "' + $(this).html() + '".'); } else { errorText = errorText.replace('This field', $(this).html()); } error.html(errorText); });

    if (isSubmit) {
        $('#error ul').append(error);
        $('#error ul>label').wrap('<li></li>');
        $('#error').show();
    }
    else {
        element.append(error);
    }
}

function validate(val, args) {
    isSubmit = true;
    $('#error ul').empty();
    args.IsValid = $('#aspnetForm').valid();
    isSubmit = false;
}

function popup(popupID) {
    $(popupID).show();
}
