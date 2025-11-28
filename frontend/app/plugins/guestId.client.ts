export default defineNuxtPlugin(() => {
  let guestId = localStorage.getItem("guestId");
  if (!guestId) {
    guestId = crypto.randomUUID();
    localStorage.setItem("guestId", guestId);
  }

  return {
    provide: {
      guestId,
    },
  };
});
