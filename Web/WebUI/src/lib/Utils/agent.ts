import { get } from "svelte/store";
import type Product from "../Models/product";
import type Profile from "../Models/profile";
import type  User  from "../Models/user";
import { jwtToken } from "../Stores/stores";

const apiUrl = "http://localhost:5000/api";

const postProducts = async (url: string, product: Product) => {
    let response;
    try{
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(product)
        });
    } catch (error) {
        console.log(error);
    }
    return response.json();
}

async function authFetch<T>(url: string, method:'POST'|'GET'|'PUT'|'DELETE', body?: any, validation:boolean=false): Promise<T>{
    const jwt = localStorage.getItem("jwt");

    const headers = {};
    
    method === 'POST' ? headers["Content-Type"] = 'application/json' : null;
    jwt ? headers["Authorization"] = `Bearer ${jwt}` : null;

    const response = await fetch(url, {
        method: method,
        headers: {...headers},
        body: body ? JSON.stringify(body) : null
    }).then(async (response) => {
        if(!response.ok){
            console.log(response.statusText)
        }
        if(validation){
            return [response.status, await response.text()] as T;
        }
        return response.status !== 204 ? 
        response.json().then(data => data as T) : 
        null;
    });
    return response;
}



function signUser(url: string, user: User) {
    return fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        }
        )
        .then(response => {
            if(!response.ok){
                console.log('signUser Error!');
            }
            return [response.status, response.text()];
        })
}

async function getUserProfile(url: string){
    return fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("jwt")}`
        }
    })
        .then(async (response) => {
            if(!response.ok){
                throw new Error(response.statusText);
            }
            const text = await response.json();
            return text as User;
        })
}

export const agent = {
    Products: {
        // getAll: async () => await getProduct<Array<Product>>(apiUrl+"/products"),
        getAll: () => authFetch<Array<Product>>(apiUrl+"/products", 'GET', null),
        // getOne: async (id: string) => await getProduct<Product>(apiUrl+"/products/"+id),
        getOne: (id: string) => authFetch<Product>(apiUrl+"/products/"+id, 'GET', null),
        post: async (product: Product) => await postProducts(apiUrl+"/products", product)
    },
    Account: {
        SignUp: (user: User) => authFetch<[number, string]>(apiUrl+"/Account/register", 'POST', user, true),
        LogIn: (user: User) => authFetch<[number, string]>(apiUrl+"/Account/login", 'POST', user, true),
        getProfile: () => authFetch<null | Profile>(apiUrl+'/Account/profile', 'GET', null),
    }
}