<svelte:head>
  <script src="https://accounts.google.com/gsi/client" async defer></script>
</svelte:head>
<script lang="ts">
  import { initUser } from "../../Stores/stores";
  import { agent } from "../../Utils/agent";

  export let showModal = false;

  window["handleCredentialResponse"] = async (response) => {
    console.log(response);
    const res = await agent.Account.logInGoogle(response.credential);
    showModal = await initUser(res, 'Login');
  }
  const client_id = import.meta.env.VITE_CLIENT_ID;

</script>

<div class="g-signin2" data-longtitle="true" data-onsuccess="onSignIn" />
<div id="g_id_onload"
    data-client_id={client_id}
    data-callback="handleCredentialResponse">
</div>
<div class="g_id_signin" data-type="standard"></div>

