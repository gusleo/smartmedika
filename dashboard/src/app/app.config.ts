export class ServiceConfiguration {
  public OAuthServer: string = "http://localhost:5000/";
  public AngularUrl: string = "http://localhost:4200/";

  private _localApi = "http://localhost:5001/admin/";
  private _liveApi = "http://52.87.109.107:5001/admin/";

  _apiUri: string;

  constructor() {
    //DEV
    //this._apiUri = this._localApi;
    //LIVE
    this._apiUri = this._liveApi;
  }

  getApiUri() {
    return this._apiUri;
  }
}
export class Configuration {
  public GoogleApiKey: string;
}

export const DATETIME_FORMAT: string = "dd-MM-yyyy HH:mm";
export const DATE_FORMAT: string = "dd-MM-yyyy";
export const FileUploadAPI: string = "http://localhost:5001/admin/file";
export const ImageNotAvailable: string = "assets/images/NotAvailable.png";
export const Config: Configuration = {
  GoogleApiKey: "AIzaSyDXvFDRtK4FkNlp65eoG-7yFjt1no7MfB0"
};

