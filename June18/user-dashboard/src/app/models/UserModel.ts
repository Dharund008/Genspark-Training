export class User
{
    constructor(public id:number=0, public username:string="",public firstName:string="",public lastName: string="",
      public gender: string="", public email:string ="",
      public image:string ="",public role:string="",public address: {state : string} = { state : ""}
    )
    {

    }

    static fromForm(data: {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    gender: string;
    image: string;
    role: string;
    address: { state: string };
  }) {
      return new User(
        data.id,
        data.username,
        data.firstName,
        data.lastName,
        data.gender, 
        data.email,
        data.image,
        data.role,
        data.address
      );
  }
}