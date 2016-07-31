$(document).ready(function()
{
    $("#add-edit-book-dialog").dialog(
    {
        autoOpen: false,
        modal: true,
        resizable: false
    });

    $("#remove-book-dialog").dialog(
    {
        autoOpen: false,
        modal: true,
        resizable: false
    });

    $("#FilteringForm input").on("change", function()
    {
        $("#FilteringForm").submit();
    });

    $("select[form='FilteringForm'").on("change", function()
    {
        $("#FilteringForm").submit();
    });
});

function goToPage(x)
{
    document.getElementById("Page").value = x;
    $("#FilteringForm").submit();
}

function addBook()
{
    $('#add-edit-book-heading').text('Add new book');
    $("#add-edit-book-id").val("");
    $("#add-edit-book-title").val("");
    $("#add-edit-book-publisher").val("");
    $("#add-edit-book-released").val("");
    $("#add-edit-book-pages").val("");
    $("#add-edit-book-cover").attr("files", null);
    $("#image").prop("outerHTML", "<img id='image' src='' class='hidden' />");
    $("#chooseImageMsg").attr("class", "");
    createHiddenFromAuthors([]);
    $('#add-edit-book-dialog').dialog('open');
    selectedFile = null;
}

function editBook(book)
{
    $('#add-edit-book-heading').text('Edit book');
    $("#add-edit-book-id").val(book.bookId);
    $("#add-edit-book-title").val(book.title);
    $("#add-edit-book-publisher").val(book.publisher);
    $("#add-edit-book-released").val(book.released);
    $("#add-edit-book-pages").val(book.pages);
    $("#add-edit-book-cover").attr("files", null);
    $("#image").prop("outerHTML", "<img id='image' src='/Books/GetCoverImage?bookId=" + book.bookId + "' />");
    $("#chooseImageMsg").attr("class", "hidden");
    createHiddenFromAuthors(book.authors);
    $("#add-edit-book-dialog").dialog("open");
    selectedFile = null;
}

function removeBook(bookId, title)
{
    $("#remove-book-id").val(bookId);
    $("#remove-book-title").html(title);
    $('#remove-book-dialog').dialog('open');
}

var selectedFile;

function sendData()
{
    var data = new FormData();
    var inputs = document.getElementById("add-edit-book-dialog").getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++)
        if (inputs[i].type != "file" && inputs[i].name)
            data.append(inputs[i].name, inputs[i].value);
    data.append("cover", selectedFile);
        
    $.ajax({
        url: '/Books/AddBook',
        cache: false,
        contentType: false,
        processData: false,
        data: data,
        dataType: "json",
        type: 'POST',
        success: function(response) 
        {
            if (response.ResponseUrl != null)
                window.location.href = response.ResponseUrl;                
        }
    });
}
    
function readURL(input)
{
    if (input.files && input.files[0])
    {
        var reader = new FileReader();
        reader.onload = function(e)
        {
            document.getElementById("image").className = "";
            document.getElementById("chooseImageMsg").className = "hidden";
            $('#image').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
        selectedFile = input.files[0];
    }
}

function getAuthorsFromHidden()
{
    var authors = [];
    var inputAuthors = document.getElementById("add-edit-book-hidden-authors-list").getElementsByTagName("input");
    for (var i = 0; i < inputAuthors.length; i++) {
        authors.push(inputAuthors[i].value);
    }
    return authors;
}

function createHiddenFromAuthors(authors) {
    var authorsElement = document.getElementById("add-edit-book-authors-list");
    authorsElement.innerHTML = "";
    for (var i = 0; i < authors.length; i++)
        authorsElement.innerHTML += "<li>" + authors[i] + "<b onclick='RemoveAuthor(" + i + ")'> &times;</b></li> ";

    var inputAuthors = document.getElementById("add-edit-book-hidden-authors-list");
    inputAuthors.innerHTML = "";
    for (var i = 0; i < authors.length; i++)
        inputAuthors.innerHTML += "<input type='hidden' id='Authors_" + i + "_' name='Authors[" + i + "]' value='" + authors[i] + "' />";
}

function AddAuthor(name) {
    if ($("#add-edit-book-author").val() != "")
    {
        var authors = getAuthorsFromHidden();
        if (authors.indexOf(name) == -1) {
            authors.push(name);
            createHiddenFromAuthors(authors);
        }
        $("#add-edit-book-author").val("");
    }
}

function RemoveAuthor(nr) {
    var authors = getAuthorsFromHidden();
    authors.splice(nr, 1);
    createHiddenFromAuthors(authors);
}