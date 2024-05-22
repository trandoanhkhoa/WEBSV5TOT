const openButton = document.getElementById('openSideBar');
const closeButton = document.getElementById('closeSideBar');
const sidebar = document.getElementById('side-bar');
const content = document.getElementById('content');
const titlesite = document.getElementById('title-site');
const footer = document.getElementById('footer');
const lightmood = document.getElementById('lightmood')
const darkmood = document.getElementById('darkmood')



// Mở side-bar
openButton.addEventListener('click', function(){
    sidebar.style.display = 'block';
});

// Đóng side-bar
closeButton.addEventListener('click', function(){
    sidebar.style.display = 'none';
});
header.addEventListener('click', function(){
    sidebar.style.display = 'none';
});
footer.addEventListener('click', function(){
    sidebar.style.display = 'none';
});
content.addEventListener('click', function(){
    sidebar.style.display = 'none';
});

// bật chế độ darkmood
darkmood.addEventListener('click', function(){
    content.style.backgroundColor = '#222';
    header.style.backgroundColor = '#222';
    sidebar.style.backgroundColor = 'rgb(18, 72, 116)';
    sidebar.style.color = '#fff';
    closeButton.style.color = '#fff';
});

lightmood.addEventListener('click', function(){
    content.style.backgroundColor = '#f1f1f1';
    header.style.backgroundColor = '#f1f1f1';
    sidebar.style.backgroundColor = '#fff';
    sidebar.style.color = '#000';
    closeButton.style.color = 'rgb(18, 72, 116)';
});
