import React, { Suspense, useEffect, useState } from "react";
import { Button, Card, Row, Col } from "react-bootstrap";
import { NAVIGATION_PATH } from "@/constants";
import { Client } from "@/types/api/Client";
import DataTable from "@/components/DataTable";
import { Link, useNavigate } from "react-router-dom";
import Loader from "@/components/Loader";
import ClientService from "@/services/ClientService";
import { TextFormFieldType } from "@/components/form/TextFormField/TextFormFieldType";


const ClientListing = () => {
    const [date, setDate] = useState<Date>();

    const navigate = useNavigate();

    useEffect(() => {

    }, []);

    return (
        <>
            <Row style={{ justifyContent: "end", margin: "10px 0" }}>
                <Link to={NAVIGATION_PATH.CLIENTS.CREATE.ABSOLUTE}>
                    <Button style={{ maxWidth: "fit-content", float: "right" }}>
                        Adicionar
                    </Button>
                </Link>
            </Row>
            <Card>
                <Card.Title></Card.Title>

                <Card.Header>
                    <Row className="align-items-center">
                        <Col>
                            <Card.Title>Clientes</Card.Title>
                        </Col>
                    </Row>
                </Card.Header>

                <Suspense fallback={<><Loader /><br /><br /></>}>
                    <DataTable<Client, any>
                        thin
                        columns={[
                            { Header: "Nome", accessor: "firstName" },
                            { Header: "Sobrenome", accessor: "lastName" },
                            { Header: "Email", accessor: "email" },
                            { Header: "Telefone", accessor: "phoneNumber" },
                            { Header: "Documento", accessor: "documentNumber" },
                        ]}
                        query={async (filters) => {
                            if (!filters || !filters[0]) {
                                console.log("Fetching all clients");
                                return await ClientService.getAll();
                            }
                            return await ClientService.getByDocument(filters[0].value.trim());
                        }}
                        fetchButton
                        cleanButton
                        filters={[
                            {
                                componentType: TextFormFieldType.INPUT,
                                name: "documentNumber",
                                label: "Buscar clientes...",
                                type: "text",
                            },
                        ]}
                        queryName={["client", "listing", date]}
                        onRowClick={(client: Client) => navigate(`${NAVIGATION_PATH.CLIENTS.EDIT.ABSOLUTE}/${client.id}`)}

                    />
                </Suspense>
            </Card>
        </>
    );
};

export default ClientListing;