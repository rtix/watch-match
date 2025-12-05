<template>
  <div class="room-handler">
    <nuxt-link to="/room-creator">
      <button>Create</button>
    </nuxt-link>
    <ui-modal>
      <template #trigger>
        <button>Join</button>
      </template>
      <ui-pin-input
        ref="room-handler__pin-input"
        @complete="tryJoiningRoom"
        @value-change="resetErrorMessage"
      />
      <div v-show="errorMessage !== ''" class="room-handler__error-message">
        {{ errorMessage }}
      </div>
    </ui-modal>
  </div>
</template>

<script lang="ts" setup>
const router = useRouter();
const { $roomService, $guestId } = useNuxtApp();
// const roomHandlerPinInput = useTemplateRef("room-handler__pin-input");
const errorMessage = ref("");

function resetErrorMessage() {
  errorMessage.value = "";
}

async function tryJoiningRoom(id: string) {
  const room = await $roomService.signToRoom(id, $guestId);
  if (!room) {
    // roomHandlerPinInput.value?.clear();
    // setTimeout(() => (errorMessage.value = "Wrong code or full"), 0);
    errorMessage.value = "Wrong code or full";
    return;
  }
  router.push({ name: "room-id", params: { id } });
}
</script>

<style scoped>
.room-handler {
  display: flex;
  gap: 1rem;
  margin-inline-start: 1rem;
}

.room-handler__error-message {
  text-align: center;
  color: crimson;
}
</style>
