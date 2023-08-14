import React, { useState } from "react";

import { Table } from "reactstrap"

import InfoEntrie from './InfoEntrie.js';

const InfoEntries = ({ entries, showData}) => {

    const [modalData, setModalData] = useState({ id: null, open: false });

    return (
        <>
            {entries.length > 0 ?
                <Table responsive>
                    <InfoEntrie modalData={modalData} setModalData={setModalData} showData={showData} />
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>TimeStamp</th>
                            <th>Rows count</th>
                            <th>Countries</th>
                            <th> </th>
                        </tr>
                    </thead>
                    {
                        entries?.map((entry) => {
                            const formattedCountries = entry?.countries.slice(0, -1).replace(/,/g, ', ') // Le saco las , al texto
                            return (
                                <tbody key={entry.id}>
                                    <tr>
                                        <td><span style={{marginLeft: "5px"} }>{entry.id}</span></td>
                                        <td> {(new Date(entry.timestamp)).toLocaleString()}</td>
                                        <td><span style={{ marginLeft: "15px" }}>{entry.count}</span></td>
                                        <td>{formattedCountries.length > 40 ?
                                            formattedCountries.slice(0,40) + "..." // En caso de que tenga muchos paises formateo la data para que no rompa el html
                                            :formattedCountries}</td>
                                        <td style={{ background: "none" }}>
                                            <button
                                                style={{ backgroundColor: "transparent", padding: "4px", color: "black", fontSize: "20px", border: "none", fontWeight: "bolder", transition: "transform 0.3s ease-in-out" }}
                                                onMouseEnter={(e) => (e.target.style.transform = "scale(1.4)")}
                                                onMouseLeave={(e) => (e.target.style.transform = "scale(1)")}
                                                onClick={() => { setModalData({ id: entry.id, open: true }) }} >+</button>
                                        </td>
                                    </tr>
                                </tbody>
                             )
                    })
                    }
                </Table>
                :
                <h4>First upload a file..</h4>
        }
        </>
    )
}

export default InfoEntries
