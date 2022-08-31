import type Product from "../Models/product";
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


function getProduct<T>(url: string): Promise<T> {
    return fetch(url)
        .then(response => {
            if(!response.ok){
                throw new Error(response.statusText);
            }
            return response.json().then(data => data as T)
        })
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
        getAll: async () => await getProduct<Array<Product>>(apiUrl+"/products"),
        getOne: async (id: string) => await getProduct<Product>(apiUrl+"/products/"+id),
        post: async (product: Product) => await postProducts(apiUrl+"/products", product)
    },
    Account: {
        SignUp: async (user: User) => signUser(apiUrl+"/Account/register", user),
        LogIn: async (user: User) => signUser(apiUrl+"/Account/login", user),
        getProfile: async () => await getUserProfile(apiUrl+'/Account/profile')
    }
}