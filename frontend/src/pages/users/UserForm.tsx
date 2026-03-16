import { Helmet } from "react-helmet-async";
import { useNavigate, useParams } from "react-router-dom";
import { NAVIGATION_PATH } from "@/constants";
import { TextFormFieldType } from "@/components/form/TextFormField/TextFormFieldType";
import { TextFormField } from "@/components/form/TextFormField/TextFormField";
import Loader from "@/components/Loader";
import { toastr } from "@/utils/toastr";
import { useSuspenseQuery } from "@tanstack/react-query";
import { ReactQueryKeys } from "@/constants/ReactQueryKeys";
import yup from "@/utils/yup";
import React, { Suspense, useState } from "react";
import { Button, Card, Col, Form, Row } from "react-bootstrap";
import { Formik } from "formik";
import { UserProfile, userProfileOptions } from "@/types/api/enums/UserProfile";
import { User } from "@/types/api/User";
import UserService from "@/services/UserService";

const INITIAL_VALUES: User = {
  id: "",
  username: "",
  password: "",
  profile: UserProfile.Operator,  
  createdAt: "",
};

const schemaValidation = yup.object().shape({
  username: yup.string().required("O nome de usuário é obrigatório"),
  password: yup.string().when("id", {
    is: (id: string) => !id || id === "",
    then: (schema) => schema.required("A senha é obrigatória para novos usuários"),
  }),
  
  profile: yup.mixed<UserProfile>().required("O perfil é obrigatório"),
});

const UserForm = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { data } = useSuspenseQuery<User>({
    queryKey: [ReactQueryKeys.USER, id],
    meta: {
      fetchFn: async () => {
        if (id) {
          const user = await UserService.getById(id);
          return {
            ...user,
             profile:
            user.profile.toString() === "Administrator"
              ? UserProfile.Administrator
              : UserProfile.Operator,
          };
        }
        return INITIAL_VALUES;
      },
    },
  });

  async function onSubmit(values: User) {
    try {
      const userToSave: User = {
        ...values,
        profile: values.profile
      };

      if (id) {
        await UserService.update(id, userToSave);
        toastr({ title: "Usuário atualizado com sucesso", icon: "success" });
      } else {
        await UserService.create(userToSave);
        toastr({ title: "Usuário criado com sucesso", icon: "success" });
      }
      navigate(NAVIGATION_PATH.USERS.LISTING.ABSOLUTE);
    } catch (err: any) {
      toastr({ title: "Erro", text: err.message, icon: "error" });
    }
  }

  const title = id ? "Editar Usuário" : "Novo Usuário";

  return (
    <React.Fragment>
      <Helmet title={title} />
      <Suspense fallback={<><Loader /><br /><br /></>}>
        <Card>
          <Card.Header>
            <Card.Title>{title}</Card.Title>
          </Card.Header>
          <Card.Body>
            <Formik
              initialValues={data}
              validationSchema={schemaValidation}
              onSubmit={onSubmit}
              enableReinitialize={true}
            >
              {({
                handleSubmit,
                handleChange,
                handleBlur,
                errors,
                values,
                isSubmitting,
                isValid,
              }) => (
                <Form noValidate onSubmit={handleSubmit}>
                  <Row>
                    <Col md={4}>
                      <TextFormField
                        componentType={TextFormFieldType.INPUT}
                        name="username"
                        label="Nome de Usuário"
                        required
                        placeholder="Nome de Usuário"
                        handleBlur={handleBlur}
                        handleChange={handleChange}
                        value={values.username}
                        formikError={errors.username}
                      />
                    </Col>
                    <Col md={4}>
                      <TextFormField
                        componentType={TextFormFieldType.SELECT}
                        name="profile"
                        label="Perfil"
                        required
                        placeholder="Selecione um perfil"
                        handleBlur={handleBlur}
                        handleChange={handleChange}
                        options={userProfileOptions()}
                        value={values.profile}
                        defaultValue={userProfileOptions().find(option => option.id === values.profile)?.id}
                        formikError={errors.profile}
                      />
                    </Col>
                    <Col md={4}>
                      <TextFormField
                        componentType={TextFormFieldType.INPUT}
                        name="password"
                        label="Senha"
                        type="password"
                        password
                        required={!id}
                        placeholder="Senha"
                        handleBlur={handleBlur}
                        handleChange={handleChange}
                        value={values.password || ""}
                        formikError={errors.password}
                      />
                    </Col>
                  </Row>
                  <br />
                  <Button type="submit" variant="primary" disabled={!isValid || isSubmitting}>
                    {isSubmitting ? "Salvando..." : "Salvar"}
                  </Button>
                  <Button
                    variant="secondary"
                    style={{ marginLeft: 5 }}
                    onClick={() => navigate(NAVIGATION_PATH.USERS.LISTING.ABSOLUTE)}
                  >
                    Voltar
                  </Button>
                </Form>
              )}
            </Formik>
          </Card.Body>
        </Card>
      </Suspense>
    </React.Fragment>
  );
};

export default UserForm;