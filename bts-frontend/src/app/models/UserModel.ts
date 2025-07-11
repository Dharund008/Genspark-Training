// export class UserModel{
// //     {
// //   "id": 1,
// //   "username": "emilys",
// //   "email": "emily.johnson@x.dummyjson.com",
// //   "firstName": "Emily",
// //   "lastName": "Johnson",
// //   "gender": "female",
// //   "image": "https://dummyjson.com/icon/emilys/128"

// // }
//     constructor(  public id:number= 0,
//   public username:string="",
//   public email:string= "",
//   public firstName:string= "",
//   public lastName:string= "",
//   public gender:string= "",
//   public image:string= "")
//   {

//   }
//   static fromForm(data:{id:number,
//   username:string,
//   email:string,
//     firstName:string,
//     lastName:string,
//   gender:string,
//   image: string}){
//         return new UserModel(data.id,data.username,data.email,data.firstName,data.lastName,data.gender,data.image);
//   }
// }

export interface User {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export enum UserRole {
  Admin = 'ADMIN',
  Developer = 'DEVELOPER',
  Tester = 'TESTER'
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
  expiresAt: Date;
}

export interface Register
{
  name: string;
  email: string;
  password: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  newPassword: string;
}
