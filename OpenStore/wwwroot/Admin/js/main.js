// Function To Display List

function displayList(element){
    var myElement = element.parentElement.children[1];
    if(myElement.classList.contains('display')){
        myElement.classList.remove('display');
        element.classList.remove('active');

    }
    else{
        var subList = document.getElementsByClassName('sub')
        for (var i = 0; i<subList.length ;i++){
            if(subList[i].classList.contains('display')){
                subList[i].classList.remove('display');
                subList[i].parentElement.children[0].classList.remove('active');
            }
        }
    myElement.classList.add('display');
    element.classList.add('active');
    }
}

// Function To Convert The Tooltips
function  convert(element){
    var sidebar = document.getElementById('sidebar');
    if(element.classList.contains('fa-bars')){
        element.classList.remove('fa-bars');
        element.classList.add('fa-times');
        sidebar.classList.add('sidebar-click');
        element.style.display = 'block';
        // sidebar.style.width = '100%';
    }
    else{
        element.classList.remove('fa-times');
        element.classList.add('fa-bars');
        sidebar.classList.remove('sidebar-click');
        // sidebar.style.width = '0';
        element.style.display = '';
    }
}
// Function To Show Commercial Number
function ifSeller(element){
    var hiddenInput = document.getElementById('commercialNumber');
    var commercialNumber = document.getElementById('commercialNumberFiled');
    if (element.value == '2'){
        hiddenInput.style.display = 'block';
    }
    else{
        console.log(commercialNumber);
        hiddenInput.style.display = 'none';
        commercialNumber.value = "";
    }
}
//Function To Show Commercial Number edit Page
window.onload = function(){
    if(commercialNumberFiled = document.getElementById("commercialNumberFiled")){
        var commercialNumberFiled = document.getElementById("commercialNumberFiled");
        var commercialNumber = document.getElementById('commercialNumber');
        var selectUser = document.getElementById('selectUser');
        if(selectUser.value != '2'){
            commercialNumberFiled.value = "";
            commercialNumber.style.display = "none";
        }
        else{
            commercialNumber.style.display = "block";
    
        }
    }
    
}

// Function TO Upload Image.
function uploadImg(element){
    var img = document.getElementById('img');
    var e = element.files;
    var div = document.getElementById('divImg');
    console.log(e);
    if(e.length>0){
        var fileReader = new FileReader();
        fileReader.onload = function (event){
            img.src = fileReader.result;
        };
        fileReader.readAsDataURL(e[0]);
        img.style.display = "block";
        div.style.backgroundColor = "inherit";
    }
}

// Function To show Dialog
function showDialog(element){
    var dialog = document.getElementById('dialog');
    var messageName = document.getElementById('messageName');
    var deleteID = document.getElementById("deleteID");
    messageName.textContent = element.getAttribute("data");
    deleteID.setAttribute('name', element.getAttribute("name"));
    deleteID.setAttribute('value', element.getAttribute("value"));
    dialog.style.visibility = "visible";
    dialog.style.opacity = "1";
    dialog.style.cursor = "auto";
    // idDialog.setAttribute('href',"delete/id?=" + elementId.getAttribute('value'));

}
// function To Show Dialog 2
function showDialog2(element){
    var dialog = document.getElementById('dialog');
    var messageName = document.getElementById('messageName');
    var deleteID = document.getElementById("deleteID");
    var FK_ID = document.getElementById("FK_ID");
    messageName.textContent = element.getAttribute("data");
    deleteID.setAttribute('name', element.getAttribute("name"));
    deleteID.setAttribute('value', element.getAttribute("value"));
    FK_ID.setAttribute('name', element.getAttribute("FK_name"));
    FK_ID.setAttribute('value', element.getAttribute("FK_value"));
    dialog.style.visibility = "visible";
    dialog.style.opacity = "1";
    dialog.style.cursor = "auto";
    // idDialog.setAttribute('href',"delete/id?=" + elementId.getAttribute('value'));

}
// function To Delete Item
function hideDialog(){
    var dialog = document.getElementById('dialog');
    dialog.style.visibility = "hidden";
    dialog.style.opacity = "0";
    dialog.style.cursor = "none";
    
} 
// Function To Chamge Img
function changeImg(element){
    var add = document.getElementById('add');
    var edit = document.getElementById('edit');
    var search = document.getElementById('search');
    var remove = document.getElementById('delete');
    console.log(element);
    add.href = add.href + element.value;
    edit.href = edit.href + element.value;
    search.href = search.href + element.value;
    remove.setAttribute('data', element.options[element.selectedIndex].textContent);
    remove.setAttribute('value', element.value);
    remove.setAttribute('name', 'id');
    console.log(element.textContent[0]);
    remove.setAttribute('data', element.options[element.selectedIndex].textContent);
    var a = document.getElementById('a');
    var img = document.getElementById('img');
    a.setAttribute('href',element.options[element.selectedIndex].getAttribute("img"));
    img.setAttribute('src',element.options[element.selectedIndex].getAttribute("img"));
    
}



// Function To Change Img 2
function changeImgAdd(element){
    var img = document.getElementById('img');
    console.log(element);
    var e = element.files;
    console.log(e);
    if(e.length>0){
        var fileReader = new FileReader();
        fileReader.onload = function (event){
            console.log(fileReader.result);
            img.src = fileReader.result;
        };
        fileReader.readAsDataURL(e[0]);   
    }
}

// Function To Show Select Options
function showSelectOption(element){
    var selectHide = element.nextElementSibling;
    selectHide.style.display = "block";
    element.setAttribute('onchange', '#');
}
// Function To Change Img 3

function changeImgAdd1(element){
    var img = document.getElementById('img');
    console.log(element);
    var e = element.files;
    console.log(e);
    if(e.length>0){
        var fileReader = new FileReader();
        fileReader.onload = function (event){
            console.log(fileReader.result);
            img.style.backgroundImage = "url('" +fileReader.result+" ')";
        };
        fileReader.readAsDataURL(e[0]);   
    }
}
// Function To Change Img 4
function changeImgAdd4(element){
    var img = element.parentElement.children[1].children[0];
    console.log(img);
    var e = element.files;
    if(e.length>0){
        var fileReader = new FileReader();
        fileReader.onload = function (event){
            img.src = fileReader.result;
        };
        fileReader.readAsDataURL(e[0]);  

    }

}

// Function To Add Img Branch
function changeImgAdd2(element){
    var imgBranch = document.getElementById('imgBranch');
    imgBranch.innerHTML = '';
        for(var i=0 ; i < element.files.length; i++){
            if(element.files.length <=4 ){
                var count = 0;
                let fileReader = new FileReader();
                var img = document.createElement('div');
                img .classList.add('col-12','col-lg-5','col-md-5','imgBranch');
                imgBranch.appendChild(img);
                fileReader.onload =()=>{
                    console.log(count);
                    imgBranch.children[count].style.backgroundImage = "url('" +fileReader.result+"')";
                    count++;
                };
                fileReader.readAsDataURL(element.files[i]);
            }
            
        }
        if(element.files.length > 4 ){
            var paragraph = document.createElement('p');
            paragraph.classList.add('text-danger');
            paragraph.textContent = "أقصى عدد صور هو 4 صور فقط";
            imgBranch.appendChild(paragraph);
            element.value = '';
        }
         
}

// Function To Change Price 
function changePrice(element){
    var totlas = document.getElementById('totals');
    var price = element.parentElement.parentElement.children[3];
    price.textContent = element.value * parseInt(price.getAttribute('data'));

}
// Function To Chamge Totlas
function totalsChange(){
    var element =  document.getElementById('tbody');
    var totals = document.getElementById('totals');
    var totalPrice = 0;
    var totalCount = 0;
    for(var i=0; i < element.children.length; i++){
        totalPrice += parseInt( element.children[i].children[3].textContent);
        totalCount += parseInt(element.children[i].children[4].firstChild.value);
    }
    totals.children[0].textContent = totalPrice;
    totals.children[1].textContent = totalCount;

}

// Function To Change Style Of Selectd Location
// function selectLocation(this){
    
// }
// Function To Show Locations
function showLocations(element){
    var Fks = document.getElementsByClassName('FK');
    console.log(Fks);
    var add = document.getElementById('add');
    var remove = document.getElementById('delete');
    var edit = document.getElementById('edit');
    var selected = document.getElementById('selected');
    var location = document.getElementById('locations');
    var province = document.getElementById('province');
    selected.textContent = element.options[element.selectedIndex].textContent;
    location.style.display = 'flex';
    province.style.display = 'flex';
    remove.setAttribute('value', element.options[element.selectedIndex].value);
    remove.setAttribute('data', element.options[element.selectedIndex].textContent);
    edit.setAttribute('value', element.options[element.selectedIndex].value);
    add.setAttribute('value', element.options[element.selectedIndex].value);
    for(var i=0; i < Fks.length; i++){
        Fks[i].setAttribute('FK_value', element.options[element.selectedIndex].value);
    }

}

// Function To change Color in Product
function changeColor(element){
    if(element.firstChild.classList.contains('star')){
        element.firstChild.classList.remove('star');
    }
    else{
        element.firstChild.classList.add('star');
    }

}













