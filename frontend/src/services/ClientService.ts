import { BaseService } from "./BaseService";
import { Client } from "@/types/api/Client";

class ClientService extends BaseService {
  constructor() {
    super("client");
  }

  async getAll(): Promise<Client[]> {
    return await this.get<Client[]>("");
  }

  async create(client: Client): Promise<string> {
    return await this.post<Client, string>("", client);
  }

  async getById(id: string): Promise<Client> {
    return await this.get<Client>(id);
  }

  async getByDocument(documentNumber: string): Promise<Client[]> {
    return await this.get<Client[]>("", { documentNumber });
  }

  async update(id: string, client: Client): Promise<void> {
    return await this.put<Client, void>(id, client);
  }

  async importCsv(file: File): Promise<void> {
    const formData = new FormData();
    formData.append('file', file);
    return await this.post<FormData, void>("import", formData);
  }

  async exportTemplate(): Promise<Blob> {
    return await this.get("Template");

  }
}

export default new ClientService();