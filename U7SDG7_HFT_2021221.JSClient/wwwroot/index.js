let locations = [];
let connection = null;
getdata();

setupSignalR();

async function getdata() {
    fetch('http://localhost:22389/location')
        .then(x => x.json())
        .then(y => {
            locations = y.$values;
            console.log(y);
            display();
        });
}

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:22389/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("LocationCreated", (user, message) => {
        getdata();
    });

    connection.on("LocationDeleted", (user, message) => {
        getdata();
    });

    connection.on("LocationUpdated", (user, message) => {
        getdata();
    });

    connection.onclose
        (async () => {
            await start();
        });
    start();

}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

function display() {
    document.getElementById('locationresult').innerHTML = "";
    locations.forEach(loc => {
        document.getElementById('locationresult').innerHTML +=
            '<tr><td>' + loc.id + 
            '</td><td>' + loc.name + 
            '</td><td>' + loc.capacity + 
            `</td><td> <button type="button" onclick="showupdate(${loc.id})">Update</button> ` +
            `<button type="button" onclick="remove(${loc.id})">Delete</button> </td></tr>`;
    });
}

//function search(id) {

//}

function create() {
    let nametmp = document.getElementById('createname').value;
    let capacitytmp = document.getElementById('createcap').value;
    fetch('http://localhost:22389/location', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            name : nametmp,
            capacity : capacitytmp
        }),
        })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

let locationId = -1;

function showupdate(id) {
    document.getElementById('updatename').value = locations.find(l => l['id'] == id)['name'];
    document.getElementById('updatediv').style.display = 'flex';
    document.getElementById('updatediv').style.flexDirection = 'column';
    locationId = id;

}

function update() {
    document.getElementById('updatediv').style.display = 'none';
    let nametmp = document.getElementById('updatename').value;
    let captmp = document.getElementById('updatecap').value;
    fetch('http://localhost:22389/location', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(
            {
                id: locationId,
                name: nametmp,
                capacity: captmp
            }),
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function remove(id) {
    fetch('http://localhost:22389/location/' + id, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

