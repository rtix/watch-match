<template>
  <div>
    <h1>Room preferences</h1>
    <button @click="createRoom">Create room</button>
  </div>
</template>

<script lang="ts" setup>
const app = useNuxtApp();
const router = useRouter();

async function createRoom() {
  try {
    const room = await app.$roomService.createRoom(app.$guestId);
    if (room.id) {
      router.push(`/room/${room.id}`);
    }
  } catch {
    console.log("Failed to create room.");
  }
}
</script>

<style></style>
