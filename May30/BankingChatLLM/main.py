from flask import Flask, request, jsonify
import torch
import json
import numpy as np
from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity

app = Flask(__name__)

class FAQService:
    def __init__(self, embeddings_path="faq_embeddings.pt", questions_path="questions.json", answers_path="answers.json"):
        self.device = "cuda" if torch.cuda.is_available() else "cpu"
        self.model = SentenceTransformer("sentence-transformers/all-mpnet-base-v2", device=self.device)
        import os
        embeddings_full_path = os.path.join(os.path.dirname(__file__), embeddings_path)
        self.embeddings = torch.load(embeddings_full_path, map_location=self.device)
        self.embeddings = self.embeddings.cpu().numpy() if self.device == "cpu" else self.embeddings.numpy()
        import os
        questions_full_path = os.path.join(os.path.dirname(__file__), questions_path)
        answers_full_path = os.path.join(os.path.dirname(__file__), answers_path)
        with open(questions_full_path, "r") as f:
            self.questions = json.load(f)
        with open(answers_full_path, "r") as f:
            self.answers = json.load(f)

    def get_answer(self, query, top_k=1):
        query_embedding = self.model.encode([query], convert_to_tensor=True)
        query_embedding = query_embedding.cpu().numpy()
        similarities = cosine_similarity(query_embedding, self.embeddings)[0]
        top_indices = similarities.argsort()[-top_k:][::-1]
        best_index = top_indices[0]
        best_score = similarities[best_index]
        return self.answers[best_index], best_score

faq_service = FAQService()

@app.route("/api/faq/ask", methods=["POST"])
def ask_faq():
    data = request.get_json()
    if not data or "question" not in data:
        return jsonify({"error": "Missing 'question' in request body"}), 400
    question = data["question"]
    answer, score = faq_service.get_answer(question)
    return jsonify({"answer": answer, "score": float(score)})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)
