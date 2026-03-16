import React, { Suspense, useEffect, useState } from "react";
import { Button, Card, Row, Col} from "react-bootstrap";
import DataTable from "@/components/DataTable";
import { Link, useNavigate } from "react-router-dom";
import Loader from "@/components/Loader";
import ImportService from "@/services/ImportService";
import { Import } from "@/types/api/Import";
import { getBadgeColorByImportStatus, getStatusByNumber, ImportStatus } from "@/types/api/enums/ImportStatus";


const ImportListing = () => {
    const [date, setDate] = useState<Date>();

    const navigate = useNavigate();

    useEffect(() => {
        setDate(new Date());
    }, []);

    return (
        <>
            <Card>
                <Card.Title></Card.Title>

                <Card.Header>
                    <Row className="align-items-center">
                        <Col>
                            <Card.Title>Importações</Card.Title>
                        </Col>
                    </Row>
                </Card.Header>

                <Suspense fallback={<><Loader /><br /><br /></>}>
                    <DataTable<Import, any>
                        thin
                        columns={[
                            { Header: "Nome do Arquivo", accessor: "fileName" },
                            { Header: "Data da Importação", accessor: "createdAt", Cell: ({ value }) => value ? new Date(value).toLocaleDateString("pt-BR") : "" },
                            { Header: "Total de Registros", accessor: "totalRows" },
                            { Header: "Registros Bem-sucedidos", accessor: "succesRows" },
                            { Header: "Registros com Erro", accessor: "failedRows" },
                            { Header: "Status", accessor: "status", Cell: ({ value }) => {
                                return getBadgeColorByImportStatus(getStatusByNumber(parseInt(value)));
                            }},
                            { Header: "% Completado", accessor: "totalProcessedRows", Cell: ({ row }) => {
                                const total = row.original.totalRows || 0;
                                const success = row.original.succesRows || 0;
                                const failed = row.original.failedRows || 0;
                                const processed = success + failed;
                                if (!total || total === 0) return "0%";
                                const percent = Math.round((processed / total) * 100);
                                return `${percent}%`;
                            } },
                        ]}
                        query={async (filters) => {
                            return await ImportService.getAll();
                        }}
                        fetchButton
                        cleanButton
                        queryName={["import", "listing", date]}
                      
                    />
                </Suspense>
            </Card>
            {}
        </>
    );
};

export default ImportListing;