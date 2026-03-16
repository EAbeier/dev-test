import React, { Suspense, useEffect, useState } from "react";
import { Button, Card, Row, Col} from "react-bootstrap";
import { NAVIGATION_PATH } from "@/constants";
import { Client } from "@/types/api/Client";
import DataTable from "@/components/DataTable";
import { Link, useNavigate } from "react-router-dom";
import Loader from "@/components/Loader";
import UserService from "@/services/UserService";
import { User } from "@/types/api/User";


const UserListing = () => {
    const [date, setDate] = useState<Date>();

    const navigate = useNavigate();

    useEffect(() => {
        setDate(new Date());
    }, []);

    return (
        <>
            <Row style={{ justifyContent: "end", margin: "10px 0" }}>
                <Link to={NAVIGATION_PATH.USERS.CREATE.ABSOLUTE}>
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
                    <DataTable<User, any>
                        thin
                        columns={[
                            { Header: "id", accessor: "id" },
                            { Header: "User Name", accessor: "username" },
                            { Header: "Profile", accessor: "profile" },
                        ]}
                        query={async (filters) => {
                            return await UserService.getAll();
                        }}
                        fetchButton
                        cleanButton
                        filters={[]}
                        queryName={["user", "listing", date]}
                        onRowClick={(user: User) => navigate(`${NAVIGATION_PATH.USERS.EDIT.ABSOLUTE}/${user.id}`)}
                      
                    />
                </Suspense>
            </Card>
            {}
        </>
    );
};

export default UserListing;