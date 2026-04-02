export interface Device {
  id?: number;
  name?: string;
  manufacturer?: string;
  type?: string;
  operatingSystem?: string;
  osVersion?: string;
  processor?: string;
  ram?: string;
  description?: string;
  userId?: number | null;
}
