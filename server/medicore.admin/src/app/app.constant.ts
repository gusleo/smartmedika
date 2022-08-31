export class AppConstant{
    public static Longitude:number = -8.6704043;
    public static Latitude: number = 115.2115357;

    
    public static AuthConfig:any = {
        authority: 'http://auth.smartmedika.com',
        client_id: 'js',
        redirect_uri: 'http://localhost:4200/auth.html',
        post_logout_redirect_uri: 'http://localhost:4200/',
        response_type: 'id_token token',
        scope: 'openid profile medicore.full_access',

        silent_redirect_uri: 'http://localhost:4200',
        automaticSilentRenew: true,
        //silentRequestTimeout:10000,
        filterProtocolClaims: true,
        loadUserInfo: true
    };
}