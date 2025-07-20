export interface User {
  id: string;
  username: string; //email field in backend
  password: string;
  role: string;
  //isDeleted: boolean;
}

export interface Admin {
  id: string;
  name: string;
  email: string;
  password: string;
  isDeleted: boolean;
}

export interface Developer {
  id: string;
  name: string;
  email: string;
  password: string;
  isDeleted: boolean;
}

export interface Tester {
  id: string;
  name: string;
  email: string;
  password: string;
  isDeleted: boolean;
}

export interface AdminRequestDTO {
  name: string;
  email: string;
  password: string;
}

export interface DeveloperRequestDTO {
  name: string;
  email: string;
  password: string;
}

export interface TesterRequestDTO {
  name: string;
  email: string;
  password: string;
}
