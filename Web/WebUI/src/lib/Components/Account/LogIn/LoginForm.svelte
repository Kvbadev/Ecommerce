<script lang="ts">

import { link } from "svelte-spa-router";
import { loop_guard } from "svelte/internal";
import {string, object} from "Yup";

const fields = ['username', 'password'];

let userSchema = object({
    username: string().required(),
    password: string().required()
});
let errors = {};

function getFormData(form) {
    const formData = new FormData(form);
    const data={};
    for(let field of formData){
        const [key, value] = field;
        data[key] = value;
    }
    return data;
}

function validateFormData(data:any, validation: {required:boolean, minLen:number, maxLen:number, regex?:string}) {
    for(const el of Object.values(data)){
        if(validation.required){
        }
    }
}

async function onSubmit (e) {
    const userData = getFormData(e.target);
    try{
        const res = validateFormData(userData, {required: true, minLen: 3, maxLen: 9})
        console.log(userData);
    } catch(err) {
        errors = err.message;
        console.log(errors);
    }
}

</script>
<div class="all">
    <div class="container">
        <form class="form" on:submit|preventDefault={onSubmit}>
            <h1>Sign In!</h1>
            <div class="fields">
            {#each fields as field}
                <input name={field} placeholder={field[0].toUpperCase()+field.slice(1)} />
                {#if errors != undefined}
                    {#if errors[0]?.split(' ')[0] == field}
                        <span class="form-error">{errors[0]}</span>
                    {/if}
                {/if}
            {/each}
            </div>
            <button class="submit" type="submit">Submit</button>
        </form>
        <div class="links">
            <a href="/Account/Register" use:link>Don't have an account?</a>
            <a href="/Account/ForgotPassword" use:link>Forgot your password?</a>
        </div>
    </div>
</div>

<style>
    .links {
        margin-top: 2rem;
        display: flex;
        flex-direction: column;
    }
    h1 {
        font-size: 5rem;
    }
    form {
        border-radius: 1rem;
        width: 100%;
        height: auto;
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    form * {
        margin: 2rem;
    }
    .fields {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    input {
        margin: 1.2rem;
        font-size: 2.8rem;
        text-align: center;
        width: 35rem;
        height: 7rem;
        border-radius: 2rem;
        border: 2px solid black;
    }
    .submit {
        font-family: 'Roboto Slab';
        border: none;
        border-radius: 2rem;
        width: 25rem;
        height: 5rem;
        cursor: pointer;
        font-size: 2.8rem;
        background-color: yellowgreen;
    }
    a {
        margin-top: auto;
        text-decoration: underline;
        color: blue;
    }
    .container {
        margin-bottom: 5rem;
        font-family: 'Rubik';
        width: auto;
        height: auto;
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    .all {
        width: 100vw;
        height: calc(100vh - 4.2vw);
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: white;
    }

</style>