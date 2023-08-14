const localUrl = "https://localhost:7107"

let headers = new Headers();

headers.append('Content-Type', 'application/json');
headers.append('Accept', 'application/json');

export async function getAllEntries() {
    try {
        const response = await fetch(
            `${localUrl}/entries`,
            {
                method: 'GET',
                header: headers,
            }
        )
        return await response.json();
    } catch (err) {
        alert("Entries not found")
    }
}

export async function getEntrie(id) {
    try {
        const response = await fetch(
            `${localUrl}/entries/${id}`,
            {
                method: 'GET',
                header: headers,
            }
        )
        return await response.json();
    } catch (err) {
        alert(err.message)
    }
}

export async function delEntrie(id) {
    try {
        const response = await fetch(
            `${localUrl}/entries/${id}`,
            {
                method: 'DELETE',
                header: headers,
            }
        )
        return await response.json();
    } catch (err) {
        alert(err.message)
    }
}

export async function postFile(file) {
    try {
        const response = await fetch(`${localUrl}/file`,
            { method: "POST", body: file, });
        return await response.json();
    } catch (err) {
        alert("Ups, upload a file in a valid format")
    }
}