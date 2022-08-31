export class StringHelper{

    /**
     * Returns string as joining text with delimiter
     * @param delimeter delimiter
     * @param params multiple string to join
     */
    public static concat(delimeter:string, ...params:any[]):string{
        let value = "";
        params.forEach(item =>{
            let str = String(item);
            if(str.length > 0)
                value += String(item).concat("/");
        });
        value = value.substring(0, value.length - 1);
        return value;
    }
}