import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class GetPromotionsUseCase {
  final PromotionRepository repository;

  GetPromotionsUseCase(this.repository);

  Future<List<Promotion>> call() async {
    return await repository.getPromotions();
  }
}