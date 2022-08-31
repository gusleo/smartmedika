export class IconCollection{
    static getIconList():Array<IconModel>{

        return Array<IconModel>(
            new IconModel("3D Rotation","3d_rotation"),
            new IconModel("Accessibility","accessibility"),
            new IconModel("Accessible","accessible"),
            new IconModel("Account Balance","account_balance"),
            new IconModel("Account Balance Wallet","account_balance_wallet"),
            new IconModel("Person Pin","person_pin"),
            new IconModel("Book","book"),
            new IconModel("Staff","group"),
            new IconModel("User","person"),
            new IconModel("Setting","settings"),
            new IconModel("Hospital","local_hospital"),
            new IconModel("Dashboard","explore")
            
            
        )
    }
}
export class IconModel{
    constructor(displayName:string, icon:string){
        this.displayName = displayName;
        this.icon = icon;
    }
    displayName:string;
    icon:string;
}