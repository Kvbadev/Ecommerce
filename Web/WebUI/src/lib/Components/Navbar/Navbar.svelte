<script lang="ts">
    import Logo from "./Logo.svelte";
    import NavItems from "./NavItems.svelte";
</script>
    
    <nav>
        <ul class="nav-items">
            <NavItems />
        </ul>
    </nav>
    
    <style>
        nav {
            width: 100vw;
            height: 10vh;
            position: sticky;
            left: 0;
            top: 0;
            background: #D85818;
            background: -webkit-linear-gradient(top left, #D85818, #F9D14D);
            background: -moz-linear-gradient(top left, #D85818, #F9D14D);
            background: linear-gradient(to bottom right, #D85818, #F9D14D);
        }    
        ul {
            width: 100%;
            height: 100%;
            display: flex;
            flex-direction: row;
            list-style-type: none;
            align-items: center;
        }
    
    </style>
    