import { BaseService } from "./BaseService";

class ImportService extends BaseService {
  constructor() {
    super("import");
  }

  async getAll(): Promise<any[]> {
    return await this.get<any[]>("");
  }
}

export default new ImportService();
