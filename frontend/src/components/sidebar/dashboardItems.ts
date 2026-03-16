import { SidebarItemsType } from "@/types/sidebar";
import { NAVIGATION_PATH } from "@/constants";
import { UserProfile } from "@/types/api/enums/UserProfile";
import {  FaRegAddressBook } from "react-icons/fa";
import { BiSolidReport } from "react-icons/bi"

// PAGES
const CLIENTS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.CLIENTS.LISTING.ABSOLUTE, title: "Clientes", icon: FaRegAddressBook }
const USUARIOS_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.USERS.LISTING.ABSOLUTE, title: "Usuários", icon: FaRegAddressBook }
const IMPORTACOES_PAGE: SidebarItemsType = { href: NAVIGATION_PATH.IMPORTS.LISTING.ABSOLUTE, title: "Importações", icon: BiSolidReport }


export const SIDEBAR = {
    [UserProfile.Administrator]: [
        {
            title: "Gestão",
            pages: [CLIENTS_PAGE, USUARIOS_PAGE, IMPORTACOES_PAGE]
        }
    ],
    [UserProfile.Operator]: [
        {
             title: "Gestão",
            pages: [CLIENTS_PAGE,IMPORTACOES_PAGE]
        }
    ]
}