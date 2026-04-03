export enum Role {
  user = 'user',
  admin = 'admin',
  support = 'support',
  manager = 'manager'

}

export interface User {
  id?: number;         
  name: string;
  role: Role;
  location: string;
  email?: string;
}