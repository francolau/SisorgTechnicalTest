import React, { useState, useEffect } from 'react'
import { Modal, ModalHeader, ModalBody, Button } from 'reactstrap'
import { delEntrie, getEntrie as getEntrieDb } from '../services/services';
import BarChart from './chart/BarChart';

const InfoEntrie = ({ modalData, setModalData, showData }) => {

    const [data, setData] = useState({})

    const getEntrie = async () => {
        try {
            const getEntrieById = await getEntrieDb(modalData.id)
            
            setData(getEntrieById.entrie)
            
        } catch (err) {
            console.log(err)
        }
    }

    const deleteEntrie = async () => {
        try {
            const delEntrieById = await delEntrie(modalData.id)

            showData();
            setModalData({ id: null, open: false });


        } catch (err) {
            console.log(err)
        }
    }

    useEffect(() => {
        if (modalData.id) {
            getEntrie();
        }
    }, [modalData])

    return (
        <Modal isOpen={modalData.open} toggle={() => setModalData({id:null, open: false }) } >
            <ModalHeader>
                Entrie info
            </ModalHeader>
                <ModalBody>
                <BarChart data={data} />
                <div style={{width:"100%", display:"flex", justifyContent:"center"} }>
                    <Button size="sm" color="danger" className="me-2 w-50 mt-3" onClick={() => deleteEntrie()} >Delete Entrie</Button>
                </div>
            </ModalBody>
        </Modal>
    )
}

export default InfoEntrie;