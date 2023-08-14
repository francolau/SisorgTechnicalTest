import { React, useEffect, useState } from 'react';

import { Container, Row, Col, Card, CardHeader, CardBody } from "reactstrap"

import { getAllEntries, postFile } from './services/services.js';

import InfoEntries from './components/InfoEntries.js';


const App = () => {

    const [entries, setEntries] = useState([])

    const showData = async () => {
        try {
            var getAllEntriesData = await getAllEntries();

            setEntries(getAllEntriesData)
        } catch (err) {
            console.log(err)
        }
    }


    async function saveEntrie(inp) {
        let formData = new FormData();
        let fileTxt = inp?.target?.files[0];

        formData.append("file", fileTxt);

        try {
            let res = await postFile(formData)
            showData();
        } catch (e) {
            alert('Houston we have problem...:', e.status);
        }
    }

    useEffect(() => {
        showData();
    }, [])

    return (
        <Container>
            <Row className="mt-5">
                <Col sm="12">
                    <Card color="dark" outline>
                        <CardHeader style={{display: "flex", alignItems: "center", justifyContent:"center"}}>
                            <h2>Entry file<span className="fs-6">.txt</span></h2>
                            <input style={{marginLeft:"15px"}} type="file" accept=".txt" onChange={(e) => saveEntrie(e) } /> 
                        </CardHeader>
                        <CardBody>
                            <hr></hr>
                            <InfoEntries entries={entries} showData={showData} />
                        </CardBody>
                    </Card>
                </Col>
            </Row>

        </Container>
    )
}

export default App;
